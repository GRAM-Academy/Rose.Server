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
    public class Scheme
    {
        public readonly string Name;

        internal Dictionary<string, Collection> Collections { get; private set; }


        //  Lock
        private RWLock _lock = new RWLock();
        public ReaderLock ReaderLock { get { return _lock.ReaderLock; } }
        public WriterLock WriterLock { get { return _lock.WriterLock; } }





        internal Scheme(string name)
        {
            //  예외문자 처리
            {
                string pattern = @"^[a-zA-Z0-9_]*$";
                if (System.Text.RegularExpressions.Regex.IsMatch(name, pattern) == false)
                    throw new AegisException(RoseResult.InvalidArgument, $"Not allowed character contains on SchemeName({name}).");
            }

            Name = name;
            Collections = new Dictionary<string, Collection>(Settings.StringComparer);
        }


        internal Collection AddCollection(string name)
        {
            if (name == null || name == "")
                throw new AegisException(RoseResult.InvalidCollectionName, "Invalid collection name.");


            using (WriterLock)
            {
                Collection collection;
                Collections.TryGetValue(name, out collection);
                if (collection != null)
                    throw new AegisException(RoseResult.DuplicateName, "The same collection name exists.");


                //  Collection 추가
                collection = new Collection(this, name, false);
                Collections.Add(name, collection);

                return collection;
            }
        }


        public Collection CreateCollection(string name, bool justInCache)
        {
            if (name == null || name == "")
                throw new AegisException(RoseResult.InvalidCollectionName, "Invalid collection name.");


            using (WriterLock)
            {
                Collection collection;
                Collections.TryGetValue(name, out collection);
                if (collection != null)
                    throw new AegisException(RoseResult.DuplicateName, "The same collection name exists.");


                //  Collection 생성
                collection = new Collection(this, name, justInCache);
                Collections.Add(name, collection);

                if (justInCache == false)
                    Storage.StorageEngine.Engine.CreateCollection(collection);

                return collection;
            }
        }


        public Collection GetCollection(string name, bool exceptionIfNotExists = true)
        {
            if (name == null || name == "")
            {
                if (exceptionIfNotExists)
                    throw new AegisException(RoseResult.InvalidCollectionName, "Invalid collection name.");
                else
                    return null;
            }


            using (ReaderLock)
            {
                Collection collection;
                Collections.TryGetValue(name, out collection);
                if (collection == null)
                {
                    if (exceptionIfNotExists)
                        throw new AegisException(RoseResult.InvalidCollectionName, "Invalid collection name.");
                    else
                        return null;
                }

                return collection;
            }
        }


        public void DeleteCollection(string name)
        {
            Collection collection;
            using (WriterLock)
            {
                Collections.TryGetValue(name, out collection);
                if (collection == null)
                    throw new AegisException(RoseResult.InvalidCollectionName, "Invalid collection name.");

                Collections.Remove(name);
            }


            if (collection.JustInCache == false)
                Storage.StorageEngine.Engine.DeleteCollection(collection);
        }


        public void GetCollectionsInfo(ref JArray destination)
        {
            using (ReaderLock)
            {
                foreach (var collection in Collections.Select(v => v.Value))
                {
                    destination.Add(new JObject()
                    {
                        { "name", collection.Name },
                        { "justInCache", collection.JustInCache }
                    });
                }
            }
        }
    }
}
