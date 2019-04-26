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
    public static class SchemeCatalog
    {
        private static Dictionary<string, Scheme> Schemes = new Dictionary<string, Scheme>(Settings.StringComparer);
        private static RWLock _lock = new RWLock();

        private static ReaderLock ReaderLock { get { return _lock.ReaderLock; } }
        private static WriterLock WriterLock { get { return _lock.WriterLock; } }





        public static void Clear()
        {
            using (WriterLock)
            {
                Schemes.Clear();
            }
        }


        public static Scheme GetScheme(string name, bool createIfNotExists = false)
        {
            if (name == null || name == "")
                throw new AegisException(RoseResult.InvalidSchemeName, "Invalid scheme name.");


            Scheme scheme;
            using (ReaderLock)
            {
                Schemes.TryGetValue(name, out scheme);
                if (scheme == null && createIfNotExists == false)
                    throw new AegisException(RoseResult.InvalidSchemeName, "Invalid scheme name.");
            }


            //  새로 생성
            if (scheme == null && createIfNotExists)
            {
                using (WriterLock)
                {
                    try
                    {
                        scheme = CreateScheme(name);
                    }
                    catch (AegisException e) when (e.ResultCodeNo == RoseResult.DuplicateName)
                    {
                        Schemes.TryGetValue(name, out scheme);
                    }
                }
            }

            return scheme;
        }


        public static void GetSchemesInfo(ref JArray destination)
        {
            using (ReaderLock)
            {
                foreach (var scheme in Schemes.Select(v => v.Value))
                {
                    destination.Add(new JObject()
                    {
                        { "schemeName", scheme.Name },
                        { "collectionCount", scheme.Collections.Count() }
                    });
                }
            }
        }


        internal static void AddScheme(string name)
        {
            if (name == null || name == "")
                throw new AegisException(RoseResult.InvalidSchemeName, "Invalid scheme name.");


            Scheme newScheme;
            using (WriterLock)
            {
                Scheme scheme;
                Schemes.TryGetValue(name, out scheme);
                if (scheme != null)
                    throw new AegisException(RoseResult.DuplicateName, $"'{name}' is already exists.");


                //  Scheme 추가
                newScheme = new Scheme(name);
                Schemes.Add(name, newScheme);
            }
        }


        public static Scheme CreateScheme(string name)
        {
            if (name == null || name == "")
                throw new AegisException(RoseResult.InvalidSchemeName, "Invalid scheme name.");


            Scheme newScheme;
            using (WriterLock)
            {
                Scheme scheme;
                Schemes.TryGetValue(name, out scheme);
                if (scheme != null)
                    throw new AegisException(RoseResult.DuplicateName, $"'{name}' is already exists.");


                //  Scheme 생성
                newScheme = new Scheme(name);
                Schemes.Add(name, newScheme);
            }


            Storage.StorageEngine.Engine.CreateScheme(newScheme);
            return newScheme;
        }


        public static void DeleteScheme(string name)
        {
            if (name == null || name == "")
                throw new AegisException(RoseResult.InvalidSchemeName, "Invalid scheme name.");


            Scheme scheme;
            using (WriterLock)
            {
                Schemes.TryGetValue(name, out scheme);
                if (scheme == null)
                    throw new AegisException(RoseResult.InvalidSchemeName, "Invalid scheme name.");

                Schemes.Remove(name);
            }

            foreach (var collectionName in scheme.Collections.Keys.ToList())
                scheme.DeleteCollection(collectionName);
            Storage.StorageEngine.Engine.DeleteScheme(scheme);
        }
    }
}
