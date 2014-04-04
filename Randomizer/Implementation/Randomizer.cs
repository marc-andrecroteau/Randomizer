using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Implementation
{
    public static class Randomizer
    {
        #region Fields

        private static readonly Random _Random = new Random();
        private static readonly IDictionary<Type, Func<object>> _Functions = InitializeMethodForEachSupportedType();

        #endregion

        #region Core Methods

        private static IDictionary<Type, Func<object>> InitializeMethodForEachSupportedType()
        {
            var dictionary = new Dictionary<Type, Func<object>>
            {
                {typeof (double), RandomizeDouble},
                {typeof (int), RandomizeInt},
                {typeof (Int64), RandomizeInt},
                {typeof (string), RandomizeString}
            };

            return dictionary;
        }

        public static T Randomize<T>(T obj)
        {
            if (obj.GetType().IsPrimitive || obj is string)
            {
                obj = TryGetValue(obj);
            }
            else
            {
                if (obj.GetType().IsGenericType)
                {
                    GenericsRandomizer.Randomize(obj);
                }
                else
                {
                    RandomizeObjectProperties(obj);
                }
            }

            return obj;
        }

        private static T TryGetValue<T>(T obj)
        {
            Func<object> function;

            if (_Functions.TryGetValue(obj.GetType(), out function))
            {
                return (T) function.Invoke();
            }

            throw new NotImplementedException("Key not found");
        }

        public static void RandomizeObjectProperties(object obj)
        {
            foreach (var propertyInfo in obj.GetType().GetProperties().Where(propertyInfo => propertyInfo.CanWrite))
            {
                propertyInfo.SetValue(obj, Randomize(propertyInfo.GetValue(obj, null)));
            }
        }

        #endregion

        #region Randomize Methods

        public static object RandomizeDouble()
        {
            return _Random.NextDouble();
        }

        public static object RandomizeInt()
        {
            return _Random.Next();
        }

        public static object RandomizeInt(int lower, int upper)
        {
            return _Random.Next(lower, upper);
        }

        public static object RandomizeString()
        {
            return _Random.NextDouble().ToString(CultureInfo.InvariantCulture);
        }

        #endregion
    }
}
