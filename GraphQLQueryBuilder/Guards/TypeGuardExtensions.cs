using System;
using GraphQLQueryBuilder.Implementations.Language;

namespace GraphQLQueryBuilder.Guards
{
    internal static class TypeGuardExtensions
    {
        public static Type MustBeGraphQLScalar(this Type type)
        {
            if (!GraphQLTypes.IsScalarType(type))
            {
                var exceptionMessage = $"The type '{type.FullName}' is not a GraphQL scalar type.";
                
                throw new InvalidOperationException(exceptionMessage)
                {
                    HelpLink = "http://spec.graphql.org/June2018/#sec-Scalars"
                };
            }

            return type;
        }

        public static Type MustBeAGraphQLObject(this Type type, string message)
        {
            if (!GraphQLTypes.IsObjectType(type))
            {
                var exceptionMessage = $"The type '{type.FullName}' is not a GraphQL object type.";
                if (!string.IsNullOrWhiteSpace(message))
                {
                    exceptionMessage += message;
                }
                
                throw new InvalidOperationException(exceptionMessage)
                {
                    HelpLink = "http://spec.graphql.org/June2018/#sec-Objects"
                };
            }

            return type;
        }

        public static Type MustNotBeGraphQLList(this Type type, string message)
        {
            if (GraphQLTypes.IsListType(type))
            {
                var exceptionMessage = $"The type '{type.FullName}' is a GraphQL list type.";
                if (!string.IsNullOrWhiteSpace(message))
                {
                    exceptionMessage += message;
                }
                
                throw new InvalidOperationException(exceptionMessage)
                {
                    HelpLink = "http://spec.graphql.org/June2018/#sec-Type-System.List"
                };
            }

            return type;
        }
    }
}