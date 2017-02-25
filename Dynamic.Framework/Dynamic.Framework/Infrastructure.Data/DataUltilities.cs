using System;
using System.Collections.Generic;
using System.Data.Metadata.Edm;
using System.Reflection;

namespace Dynamic.Framework.Infrastructure.Data
{
    public static class DataUltilities
    {
        private static object _synObj = new object();
        private static Dictionary<string, MetadataWorkspace> dicMetaData = new Dictionary<string, MetadataWorkspace>();

        public static MetadataWorkspace GetMetaData(string entityName)
        {
            lock (DataUltilities._synObj)
            {
                if (!DataUltilities.dicMetaData.ContainsKey(entityName))
                {
                    string local_0 = string.Format("res://*/{0}.csdl|res://*/{0}.ssdl|res://*/{0}.msl", (object)entityName);
                    DataUltilities.dicMetaData.Add(entityName, new MetadataWorkspace((IEnumerable<string>)local_0.Split('|'), (IEnumerable<Assembly>)AppDomain.CurrentDomain.GetAssemblies()));
                }
            }
            return DataUltilities.dicMetaData[entityName];
        }
    }
}
