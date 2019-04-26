using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Aegis.Threading;

namespace Rose.Engine.Cache
{
    [DebuggerDisplay("Indexes={_indexes}")]
    internal class IndexMap
    {
        private RWLock _lock = new RWLock();
        private readonly Collection ParentCollection;
        private readonly Dictionary<string, IndexChunk> _indexes;
        private readonly Dictionary<string, List<IndexChunk>> _chunkListByObjectId;





        public IndexMap(Collection collection)
        {
            ParentCollection = collection;
            _indexes = new Dictionary<string, IndexChunk>();
            _chunkListByObjectId = new Dictionary<string, List<IndexChunk>>();
        }


        public List<string> GetIndexNames()
        {
            using (_lock.ReaderLock)
            {
                return _indexes.Keys.ToList();
            }
        }


        public void Add(string indexName, string objectId)
        {
            //  objectId를 Chunk에 추가
            IndexChunk chunk;
            using (_lock.ReaderLock)
                _indexes.TryGetValue(indexName, out chunk);

            if (chunk == null)
            {
                //  Double lock checking
                using (_lock.WriterLock)
                {
                    _indexes.TryGetValue(indexName, out chunk);
                    if (chunk == null)
                    {
                        chunk = new IndexChunk();
                        _indexes.Add(indexName, chunk);
                    }
                }
            }

            chunk.Add(objectId);


            //  objectId에 연관된 Chunk로 추가
            using (_lock.WriterLock)
            {
                List<IndexChunk> chunkList;
                _chunkListByObjectId.TryGetValue(objectId, out chunkList);
                if (chunkList == null)
                {
                    chunkList = new List<IndexChunk>();
                    _chunkListByObjectId.Add(objectId, chunkList);
                }

                if (chunkList.Contains(chunk) == false)
                    chunkList.Add(chunk);
            }
        }


        public void Remove(string indexName, string objectId)
        {
            IndexChunk chunk;
            using (_lock.ReaderLock)
                _indexes.TryGetValue(indexName, out chunk);

            if (chunk != null)
                chunk.Remove(objectId);


            //  objectId에 연관된 Chunk에서 삭제
            using (_lock.WriterLock)
            {
                List<IndexChunk> chunkList;
                _chunkListByObjectId.TryGetValue(objectId, out chunkList);
                if (chunkList != null)
                    chunkList.Remove(chunk);
            }
        }


        public void RemoveData(List<DataObject> candidates)
        {
            using (_lock.WriterLock)
            {
                foreach (var item in candidates)
                {
                    List<IndexChunk> chunkList;
                    if (_chunkListByObjectId.TryGetValue(item.ObjectId, out chunkList) == true)
                    {
                        foreach (var chunk in chunkList)
                            chunk.Remove(item.ObjectId);
                    }
                }
            }

        }


        public void RemoveIndex(string indexName)
        {
            IndexChunk chunk;
            using (_lock.ReaderLock)
            {
                if (_indexes.TryGetValue(indexName, out chunk) == false)
                    return;
            }

            using (_lock.WriterLock)
            {
                _indexes.Remove(indexName);


                List<string> candidates = new List<string>();
                foreach (var item in _chunkListByObjectId)
                {
                    if (item.Value.Contains(chunk) == true)
                        candidates.Add(item.Key);
                }

                foreach (var key in candidates)
                    _chunkListByObjectId.Remove(key);
            }
        }


        public IndexChunk GetChunk(string indexName)
        {
            IndexChunk chunk = null;
            using (_lock.ReaderLock)
                _indexes.TryGetValue(indexName, out chunk);

            return chunk;
        }
    }





    [DebuggerDisplay("{_objectIdList}")]
    internal class IndexChunk
    {
        private readonly List<string> _objectIdList;





        public IndexChunk()
        {
            _objectIdList = new List<string>();
        }


        public int Count()
        {
            return _objectIdList.Count();
        }


        public void Add(string objectId)
        {
            _objectIdList.Add(objectId);
        }


        public void Remove(string objectId)
        {
            _objectIdList.Remove(objectId);
        }


        public bool Contains(string objectId)
        {
            if (_objectIdList.Find(v => v == objectId) != null)
                return true;

            return false;
        }


        public List<string> GetAllObjectId()
        {
            List<string> result = _objectIdList.ToList();
            return result;
        }
    }
}
