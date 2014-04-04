using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Model;

namespace Implementation
{
    internal class GenericsRandomizer
    {
        #region Fields

        private static readonly IDictionary<Type, Func<object, object>> _Functions = InitializeMethodForEachSupportedType();

        #endregion

        #region Core Methods

        private static IDictionary<Type, Func<object, object>> InitializeMethodForEachSupportedType()
        {
            var dictionary = new Dictionary<Type, Func<object, object>>
            {
                {typeof (List<Foo>), RandomizeList},
                {typeof (List<Bar>), RandomizeList},
                {typeof (List<List<Bar>>), RandomizeList},
                {typeof (List<IBar>), RandomizeList},
                {typeof (IList<Bar>), RandomizeList}
            };

            return dictionary;
        }

        public static T Randomize<T>(T obj)
        {
            if (!obj.GetType().IsGenericType)
            {
                throw new ArgumentException("Not a generic type");
            }

            obj = TryGetValue(obj);

            return obj;
        }

        private static T TryGetValue<T>(T obj)
        {
            Func<object, object> function;

            if (_Functions.TryGetValue(obj.GetType(), out function))
            {
                return (T) function.Invoke(obj);
            }

            throw new NotImplementedException("Key not found");
        }

        #endregion

        #region Randomize Methods

        private static object RandomizeList(object obj)
        {
            var genericArgumentTypes = obj.GetType().GetGenericArguments();
            var list = (IList) obj;

            for (int i = 0; i < (int) Randomizer.RandomizeInt(0, 10); i++)
            {
                foreach (var genericArgumentType in genericArgumentTypes)
                {
                    var type = genericArgumentType;

                    if (CanInstance(genericArgumentType))
                    {
                        type = GetInstanciableType(genericArgumentType);
                    }

                    var instance = Activator.CreateInstance(type);
                    list.Add(Randomizer.Randomize(instance));
                }
            }

            return list;
        }

        #endregion

        #region Helpers

        private static bool CanInstance(Type genericArgumentType)
        {
            return genericArgumentType.IsInterface || genericArgumentType.IsAbstract;
        }

        private static Type GetInstanciableType(Type interfaceType)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (interfaceType.IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
                    {
                        return type;
                    }
                }
            }

            throw new ArgumentException("No concrete class linked to " + interfaceType.Name);
        }

        #endregion
    }
}