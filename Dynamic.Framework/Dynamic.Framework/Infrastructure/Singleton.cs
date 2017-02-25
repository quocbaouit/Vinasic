using System;
using System.Collections.Generic;

namespace Dynamic.Framework.Infrastructure
{
    public class Singleton
    {
        private static readonly IDictionary<Type, object> allSingletons = (IDictionary<Type, object>)new Dictionary<Type, object>();

        public static IDictionary<Type, object> AllSingletons
        {
            get
            {
                return Singleton.allSingletons;
            }
        }
    }

    public class Singleton<T> : Singleton
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                return Singleton<T>.instance;
            }
            set
            {
                Singleton<T>.instance = value;
                Singleton.AllSingletons[typeof(T)] = (object)value;
            }
        }
    }
}
