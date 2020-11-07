using System;
using System.Collections;

namespace GraphQLQueryBuilder.Implementations.Language
{
    internal static class GraphQLTypes
    {
        public static bool IsScalarType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type == typeof(string) || (!type.IsClass && !type.IsInterface);
        }

        public static bool IsObjectType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return !IsScalarType(type);
        }

        public static bool IsListType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return typeof(IEnumerable).IsAssignableFrom(type);
        }
    }
}