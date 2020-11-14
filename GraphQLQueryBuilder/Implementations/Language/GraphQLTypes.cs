using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using GraphQLQueryBuilder.Abstractions.Language;

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

            return typeof(GraphQLType).GetMember(type.ToGraphQLType().ToString())
                .Any(x => x.GetCustomAttributes(typeof(IsScalarAttribute)).Any());
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