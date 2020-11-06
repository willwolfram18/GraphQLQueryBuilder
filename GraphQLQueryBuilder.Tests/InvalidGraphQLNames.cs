using System.Collections.Generic;
using NUnit.Framework;

namespace GraphQLQueryBuilder.Tests
{
    public static class InvalidGraphQLNames
    {
        public static IEnumerable<string[]> InvalidOperationNames =>
            new []
            {
                new [] { null, "because name is null" },
                new [] { "", "because name is empty" },
                new [] { "   ", "because name is only white space" },
                new [] { "  \n \t ", "because name is only white space" },
                new [] { "1BigQuery", "because name starts with a number" },
                new [] { "Operation Name", "because name contains spaces"},
                new [] { "Illegal+Characters&Stars*", "because name contains illegal characters"}
            };
    }

    public class BadGraphQLNamesTestCaseSourceAttribute : TestCaseSourceAttribute
    {
        public BadGraphQLNamesTestCaseSourceAttribute() : base(typeof(InvalidGraphQLNames), nameof(InvalidGraphQLNames.InvalidOperationNames))
        {
        }
    }
}