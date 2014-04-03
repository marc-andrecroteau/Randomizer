using System;
using System.Collections;
using System.Collections.Generic;
using Model;

namespace Implementation
{
    internal class GenericsRandomizer
    {
        private static readonly IDictionary<Type, Func<object, object>> _Functions = InitializeMethodForEachSupportedType();

        private static IDictionary<Type, Func<object, object>> InitializeMethodForEachSupportedType()
        {
            var dictionary = new Dictionary<Type, Func<object, object>>
            {
                {typeof(List<Foo>), RandomizeList},
                {typeof(List<Bar>), RandomizeList},
                {typeof(List<List<Bar>>), RandomizeList},
                {typeof(IList<Bar>), RandomizeList}
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
                return (T)function.Invoke(obj);
            }

            throw new NotImplementedException("Key not found");
        }

        private static object RandomizeList(object obj)
        {
            var arguments = obj.GetType().GetGenericArguments();
            var list = (IList)obj;

            for (int i = 0; i < (int)Randomizer.RandomizeInt(0, 10); i++)
            {
                foreach (var argument in arguments)
                {
                    var instance = Activator.CreateInstance(argument);
                    list.Add(Randomizer.Randomize(instance));
                }
            }

            return list;
        }
    }
}