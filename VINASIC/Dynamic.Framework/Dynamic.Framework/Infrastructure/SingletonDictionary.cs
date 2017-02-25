﻿using System.Collections.Generic;

namespace Dynamic.Framework.Infrastructure
{
    public class SingletonDictionary<TKey, TValue> : Singleton<IDictionary<TKey, TValue>>
    {
        public new static IDictionary<TKey, TValue> Instance
        {
            get
            {
                return (IDictionary<TKey, TValue>)Singleton<Dictionary<TKey, TValue>>.Instance;
            }
        }

        static SingletonDictionary()
        {
            Singleton<Dictionary<TKey, TValue>>.Instance = new Dictionary<TKey, TValue>();
        }
    }
}
