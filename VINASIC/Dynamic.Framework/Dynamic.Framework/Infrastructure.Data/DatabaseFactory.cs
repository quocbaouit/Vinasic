using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Reflection;

namespace Dynamic.Framework.Infrastructure.Data
{
    public class DatabaseFactory
    {
        private static object _synObj = new object();
        private static Dictionary<string, object> _dicDataContext;

        public static T CreateDatabase<T>(string connectionString) where T : DbContext
        {
            lock (DatabaseFactory._synObj)
                return DatabaseFactory.CreateDatabase<T>(connectionString, connectionString, false);
        }

        public static T CreateDatabase<T>(string name, string connectionString, bool forceRecreate = false) where T : DbContext
        {
            DatabaseFactory.Initialize();
            if (forceRecreate && DatabaseFactory._dicDataContext.ContainsKey(name))
                DatabaseFactory._dicDataContext.Remove(name);
            if (!DatabaseFactory._dicDataContext.ContainsKey(name))
            {
                Type type = typeof(T);
                DatabaseFactory._dicDataContext.Add(name, type.InvokeMember(type.Name, BindingFlags.CreateInstance, (Binder)null, (object)null, new object[1]
        {
          (object) connectionString
        }));
            }
            return DatabaseFactory._dicDataContext[name] as T;
        }

        private static void Initialize()
        {
            if (DatabaseFactory._dicDataContext != null)
                return;
            DatabaseFactory._dicDataContext = new Dictionary<string, object>();
        }
    }
}
