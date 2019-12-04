using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQLQueryBuilder.Tests.Models;
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

        [Fact]
        public void IfQueryRootWithOperationNameBuilderIsUsedThenChildQueriesAppear()
        {
            var addressQuery = new QueryBuilder<Address>("address")
                .AddField(address => address.Street1)
                .AddField(address => address.Street2)
                .AddField(address => address.City)
                .AddField(address => address.State)
                .AddField(address => address.ZipCode);

            var query = new QueryRootWithOperationNameBuilder("CompleteAddressOperation")
                .AddQuery(addressQuery)
                .Build();

            Snapshot.Match(query);
        }

        [Fact]
        public void IfQueryRootWithOperationNameBuilderIsUsedWithAliasedChildQueryThenAliasedChildQueryAppears()
        {
            var addressQuery = new QueryBuilder<Address>("address")
                .AddField(address => address.Street1)
                .AddField(address => address.Street2)
                .AddField(address => address.City)
                .AddField(address => address.State)
                .AddField(address => address.ZipCode);

            var query = new QueryRootWithOperationNameBuilder("CompleteAddressOperation")
                .AddQuery("theAddress", addressQuery)
                .Build();

            Snapshot.Match(query);
        }
    }
}
