using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis;
using Aegis.Threading;
using Aegis.Data.Json;
using Newtonsoft.Json.Linq;

namespace Rose.Engine.Cache
{
    internal class CachedObjects
    {
        //  Lock
        private RWLock _lock = new RWLock();


        private readonly Collection ParentCollection;
        private readonly Dictionary<string, DataObject> _objectsByObjectId;





        public CachedObjects(Collection collection)
        {
            ParentCollection = collection;
            _objectsByObjectId = new Dictionary<string, DataObject>();
        }


        public void Initialize()
        {
        }


        public void Release()
        {
        }


        public void Add(string objectId, JToken data)
        {
            using (_lock.WriterLock)
            {
                if (_objectsByObjectId.ContainsKey(objectId) == true)
                    return;

                _objectsByObjectId.Add(objectId, DataObject.NewObject(objectId, data));
            }
        }


        public DataObject GetData(string objectId)
        {
            using (_lock.ReaderLock)
            {
                DataObject obj;
                if (_objectsByObjectId.TryGetValue(objectId, out obj) == false)
                    return null;


                if (obj == null)
                {
                    //  Storage에서 읽어오지만 Cached에 저장하지는 않는다.
                    string data = Storage.StorageEngine.Engine.ReadData(ParentCollection, objectId);
                    if (data != null)
                        obj = DataObject.NewObject(objectId, data);
                }

                return obj;
            }
        }


        public Dictionary<string, DataObject> GetAllData()
        {
            using (_lock.ReaderLock)
            {
                var result = new Dictionary<string, DataObject>();
                foreach (var item in _objectsByObjectId)
                {
                    if (item.Value == null)
                    {
                        //  Storage에서 읽어오지만 Cached에 저장하지는 않는다.
                        string data = Storage.StorageEngine.Engine.ReadData(ParentCollection, item.Key);
                        if (data != null)
                            _objectsByObjectId[item.Key] = DataObject.NewObject(item.Key, data);
                    }

                    result.Add(item.Key, item.Value);
                }

                return result;
            }
        }


        public List<String> GetAllObjectId()
        {
            using (_lock.ReaderLock)
            {
                return _objectsByObjectId.Keys.ToList();
            }
        }


        public void Remove(List<DataObject> candidates)
        {
            using (_lock.WriterLock)
            {
                foreach (var item in candidates)
                    _objectsByObjectId.Remove(item.ObjectId);
            }
        }
    }
}
