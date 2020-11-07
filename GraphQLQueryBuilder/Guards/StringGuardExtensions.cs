using System;
using GraphQLQueryBuilder.Implementations.Language;

namespace GraphQLQueryBuilder.Guards
{
    internal static class StringGuardExtensions
    {
        public static string MustBeValidGraphQLName(this string name, string paramName)
        {
            name = name?.Trim();
            
            if (!string.IsNullOrWhiteSpace(name) && !GraphQLName.IsValid(name))
            {
                throw new ArgumentException("Provided name does not comply with GraphQL's Name specification.", paramName)
                {
                    HelpLink = "https://spec.graphql.org/June2018/#Name"
                };
            }

            return name;
        }
    }
}