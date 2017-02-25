using System.Collections.Generic;

namespace Dynamic.Framework.Infrastructure
{
    public class SingletonList<T> : Singleton<IList<T>>
    {
        public new static IList<T> Instance
        {
            get
            {
                return Singleton<IList<T>>.Instance;
            }
        }

        static SingletonList()
        {
            Singleton<IList<T>>.Instance = (IList<T>)new List<T>();
        }
    }
}
