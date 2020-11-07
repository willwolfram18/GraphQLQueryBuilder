using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace GraphQLQueryBuilder.Tests
{
    public static class InvalidGraphQLNames
    {
        public static IEnumerable<string[]> InvalidNames =>
            new []
            {
                new [] { "1BigQuery", "because name starts with a number" },
                new [] { "Operation Name", "because name contains spaces"},
                new [] { "Illegal+Characters&Stars*", "because name contains illegal characters"}
            };

        public static IEnumerable<string[]> InvalidAliasNames =>
            InvalidNames.Where(values => !string.IsNullOrWhiteSpace(values[0]));
    }

    public class InvalidOperationNamesTestCaseSourceAttribute : TestCaseSourceAttribute
    {
        public InvalidOperationNamesTestCaseSourceAttribute() : base(typeof(InvalidGraphQLNames), nameof(InvalidGraphQLNames.InvalidNames))
        {
        }
    }

    public class InvalidAliasNamesTestCaseSourceAttribute : TestCaseSourceAttribute
    {
        public InvalidAliasNamesTestCaseSourceAttribute() : base(typeof(InvalidGraphQLNames), nameof(InvalidGraphQLNames.InvalidAliasNames))
        {
        }
    }
}