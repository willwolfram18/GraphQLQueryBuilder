using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snapshooter.Xunit;
using Xunit;

namespace GraphQLQueryBuilder.Tests
{
    public class OperationNameTests
    {
        [Fact]
        public void IfOperationNameIsAddedToQueryRootBuilderThenOperationNameIsInQueryDefinition()
        {
            var query = new QueryRootBuilder()
                .AddOperationName("ExampleOperation")
                .Build();

            Snapshot.Match(query);
        }

        [Fact]
        public void IfQueryRootWithOperationNameBuilderIsUsedThenOperationNameIsInTheQueryDefinition()
        {
            var query = new QueryRootWithOperationNameBuilder("NamedOperation")
                .Build();

            Snapshot.Match(query);
        }
    }
}
