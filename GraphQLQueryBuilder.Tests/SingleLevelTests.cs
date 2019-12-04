using GraphQLQueryBuilder.Tests.Models;
using Snapshooter.Xunit;
using Xunit;

namespace GraphQLQueryBuilder.Tests
{
    public class SingleLevelTests
    {
        [Fact]
        public void EmptyQuery()
        {
            var query = new QueryRootBuilder()
                .Build();

            Snapshot.Match(query);
        }

        [Fact]
        public void SimpleAddressQuery()
        {
            var addressQuery = new QueryBuilder<Address>("address")
                .AddField(address => address.Street1)
                .AddField(address => address.Street2)
                .AddField(address => address.City)
                .AddField(address => address.State)
                .AddField(address => address.ZipCode);

            var query = new QueryRootBuilder()
                .AddQuery(addressQuery)
                .Build();

            Snapshot.Match(query);
        }

        [Fact]
        public void RepeatedPropertiesIncludedInQuery()
        {
            var addressQuery = new QueryBuilder<Address>("address")
                .AddField(address => address.Street1)
                .AddField(address => address.Street1)
                .AddField(address => address.Street1)
                .AddField(address => address.Street2)
                .AddField(address => address.City)
                .AddField(address => address.State)
                .AddField(address => address.ZipCode);

            var query = new QueryRootBuilder()
                .AddQuery(addressQuery)
                .Build();

            Snapshot.Match(query);
        }
    }
}