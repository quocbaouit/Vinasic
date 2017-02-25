using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Dynamic.Framework.Generic
{
    public class Dynamic
    {
        private Dictionary<string, object> dictionary = new Dictionary<string, object>();

        public Dynamic Add<T>(string key, T value)
        {
            TypeBuilder typeBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("DynamicAssembly"), AssemblyBuilderAccess.Run).DefineDynamicModule("Dynamic.dll").DefineType(Guid.NewGuid().ToString());
            typeBuilder.SetParent(this.GetType());
            PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(key, PropertyAttributes.None, typeof(T), Type.EmptyTypes);
            MethodBuilder mdBuilder = typeBuilder.DefineMethod("get_" + key, MethodAttributes.Public, CallingConventions.HasThis, typeof(T), Type.EmptyTypes);
            ILGenerator ilGenerator = mdBuilder.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldstr, key);
            ilGenerator.Emit(OpCodes.Callvirt, typeof(Dynamic).GetMethod("Get", BindingFlags.Instance | BindingFlags.NonPublic).MakeGenericMethod(typeof(T)));
            ilGenerator.Emit(OpCodes.Ret);
            propertyBuilder.SetGetMethod(mdBuilder);
            Dynamic dynamic = (Dynamic)Activator.CreateInstance(typeBuilder.CreateType());
            dynamic.dictionary = this.dictionary;
            this.dictionary.Add(key, (object)value);
            return dynamic;
        }

        protected T Get<T>(string key)
        {
            return (T)this.dictionary[key];
        }
    }
}
