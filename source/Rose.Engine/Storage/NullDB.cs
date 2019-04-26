using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis;
using Aegis.Threading;
using Aegis.Data;
using Aegis.Data.MySQL;
using Aegis.Calculate;
using Rose.Engine.Cache;

namespace Rose.Engine.Storage
{
    internal class NullDB : StorageEngine
    {
        public override int QueuedJobCount { get { return 0; } }





        protected override void InitEngine(TreeNode<string> config)
        {
        }


        protected override void ReleaseEngine()
        {
        }


        protected override void CheckStorage(TreeNode<string> config)
        {
        }


        protected override void CreateStorage(TreeNode<string> config)
        {
        }


        internal override void CreateScheme(Scheme scheme)
        {
        }


        internal override void DeleteScheme(Scheme scheme)
        {
        }


        internal override void CreateCollection(Collection collection)
        {
        }


        internal override void DeleteCollection(Collection collection)
        {
        }


        internal override void AddIndex(Collection collection, string indexName)
        {
        }


        internal override void DeleteIndex(Collection collection, string indexName)
        {
        }


        internal override void InsertData(Collection collection, List<DataObject> dataList)
        {
        }


        internal override void UpdateData(Collection collection, List<DataObject> dataList)
        {
        }


        internal override void DeleteData(Collection collection, List<DataObject> dataList)
        {
        }


        internal override string ReadData(Collection collection, string objectId)
        {
            return null;
        }
    }
}
