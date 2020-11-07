using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace GraphQLQueryBuilder.Tests.QueryOperationBuilderTests
{
    [TestFixtureSource(nameof(OperationTypes))]
    public abstract class QueryOperationBuilderTest
    {
        protected QueryOperationBuilderTest(GraphQLOperationType operationType)
        {
            OperationTypeForFixture = operationType;
        }
        
        protected GraphQLOperationType OperationTypeForFixture { get; }
        
        private static IEnumerable<GraphQLOperationType> OperationTypes => Enum.GetValues(typeof(GraphQLOperationType))
            .Cast<GraphQLOperationType>();
    }
}