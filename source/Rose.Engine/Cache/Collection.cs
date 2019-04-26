using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis;
using Aegis.Threading;
using Newtonsoft.Json.Linq;

namespace Rose.Engine.Cache
{
    public class Collection
    {
        public readonly Scheme ParentScheme;
        public readonly string Name;
        public readonly bool JustInCache;

        internal CachedObjects CachedObjects { get; private set; }
        internal IndexMap IndexMap { get; private set; }
        private List<string> _indexKeys;


        //  Lock
        private RWLock _lock = new RWLock();
        public ILockObject ReaderLock { get { return _lock.ReaderLock; } }
        public ILockObject WriterLock { get { return _lock.WriterLock; } }

        //  Events
        internal event Action<Collection, DataObject> AfterObjectAdd;





        public static Collection GetCollection(string name)
        {
            string[] target = name.Split('.');
            if (target.Count() != 2)
                throw new AegisException(RoseResult.InvalidArgument, "Collection must be in {scheme_name}.{collection_name} format.");

            return SchemeCatalog.GetScheme(target[0].Trim())?.GetCollection(target[1].Trim());
        }


        internal Collection(Scheme scheme, string name, bool justInCache)
        {
            //  예외문자 처리
            {
                string pattern = @"^[a-zA-Z0-9_]*$";
                if (System.Text.RegularExpressions.Regex.IsMatch(name, pattern) == false)
                    throw new AegisException(RoseResult.InvalidArgument, $"Not allowed character contains on CollectionName({name}).");
            }


            if (scheme.GetCollection(name, false) != null)
                throw new AegisException(RoseResult.DuplicateName, "The same collection name exists.");

            CachedObjects = new CachedObjects(this);
            IndexMap = new IndexMap(this);
            _indexKeys = new List<string>();

            ParentScheme = scheme;
            Name = name;
            JustInCache = justInCache;

            AfterObjectAdd += OnDataAdded;
        }


        internal Collection(Scheme scheme, string name, string jsonText, bool justInCache)
        {
            CachedObjects = new CachedObjects(this);
            IndexMap = new IndexMap(this);
            _indexKeys = new List<string>();

            ParentScheme = scheme;
            Name = name;
            JustInCache = justInCache;

            AfterObjectAdd += OnDataAdded;
        }


        private string GenerateObjectId()
        {
            return Guid.NewGuid().ToString("N");
        }


        private void OnDataAdded(Collection collection, DataObject data)
        {
            //  인덱스 키가 포함되었는지 여부 확인
            using (ReaderLock)
            {
                foreach (string key in _indexKeys)
                {
                    if (data.ContainsKey(key) == true)
                    {
                        //  IndexMap에 추가
                        IndexMap.Add(key, data.ObjectId);
                    }
                }
            }
        }


        private void OnDataRemoving(Collection collection, DataObject data)
        {
            //  인덱스 키가 포함되었는지 여부 확인
            using (ReaderLock)
            {
                foreach (string key in _indexKeys)
                {
                    if (data.ContainsKey(key) == true)
                    {
                        //  IndexMap에서 제거
                        IndexMap.Remove(key, data.ObjectId);
                    }
                }
            }
        }


        internal void AddIndexForInit(string index)
        {
            //  데이터 전체를 Scan하여 해당 Index를 갖고있는 객체를 모두 IndexMap에 추가
            using (WriterLock)
            {
                if (_indexKeys.Contains(index) == true)
                    return;

                _indexKeys.Add(index);


                var collectionData = CachedObjects.GetAllData();
                foreach (var data in collectionData.Values)
                {
                    if (data.ContainsKey(index) == true)
                        IndexMap.Add(index, data.ObjectId);
                }
            }
        }


        public void AddIndex(string index)
        {
            //  데이터 전체를 Scan하여 해당 Index를 갖고있는 객체를 모두 IndexMap에 추가
            using (WriterLock)
            {
                if (_indexKeys.Contains(index) == true)
                    return;

                _indexKeys.Add(index);


                var collectionData = CachedObjects.GetAllData();
                foreach (var data in collectionData.Values)
                {
                    if (data.ContainsKey(index) == true)
                        IndexMap.Add(index, data.ObjectId);
                }
            }

            Storage.StorageEngine.Engine.AddIndex(this, index);
        }


        public void DeleteIndex(string index)
        {
            using (WriterLock)
                _indexKeys.Remove(index);

            IndexMap.RemoveIndex(index);
            Storage.StorageEngine.Engine.DeleteIndex(this, index);
        }


        public bool ContainsIndex(string index)
        {
            using (ReaderLock)
            {
                foreach (string key in _indexKeys)
                {
                    if (key == index)
                        return true;
                }
            }

            return false;
        }


        internal void AddData(string objectId, string data)
        {
            DataObject obj = DataObject.NewObject(objectId, data);

            CachedObjects.Add(objectId, JToken.Parse(data));
            AfterObjectAdd(this, obj);
        }


        internal DataObject AddData(JToken data)
        {
            string objectId = GenerateObjectId();
            DataObject obj = DataObject.NewObject(objectId, data);

            CachedObjects.Add(objectId, data);
            AfterObjectAdd(this, obj);

            return obj;
        }


        internal void RemoveData(List<DataObject> candidates)
        {
            using (WriterLock)
            {
                IndexMap.RemoveData(candidates);
                CachedObjects.Remove(candidates);
            }
        }


        internal Dictionary<string, DataObject> GetObjects(string index)
        {
            List<string> objectIds;
            if (index != null && ContainsIndex(index) == true)
            {
                objectIds = IndexMap.GetChunk(index)?.GetAllObjectId();
                Logger.Debug("dbg Indexed scan {0}.", index);
            }
            else
            {
                objectIds = CachedObjects.GetAllObjectId();
                Logger.Debug("dbg Dirty scan {0}.", index);
            }


            return objectIds.ToDictionary(v => v, v => CachedObjects.GetData(v));
        }
    }
}
