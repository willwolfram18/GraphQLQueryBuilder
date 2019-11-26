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
            var query = new QueryBuilder<Customer>()
                .Build();

            Snapshot.Match(query);
        }

        [Fact]
        public void SimpleAddressQuery()
        {
            var addressQuery = new QueryBuilder<Address>("address")
                .AddProperty(address => address.Street1)
                .AddProperty(address => address.Street2)
                .AddProperty(address => address.City)
                .AddProperty(address => address.State)
                .AddProperty(address => address.ZipCode);

            var query = new QueryBuilder<Customer>()
                .AddQuery(addressQuery)
                .Build();

            Snapshot.Match(query);
        }

        [Fact]
        public void RepeatedPropertiesIncludedInQuery()
        {
            var addressQuery = new QueryBuilder<Address>("address")
                .AddProperty(address => address.Street1)
                .AddProperty(address => address.Street1)
                .AddProperty(address => address.Street1)
                .AddProperty(address => address.Street2)
                .AddProperty(address => address.City)
                .AddProperty(address => address.State)
                .AddProperty(address => address.ZipCode);

            var query = new QueryBuilder<Customer>()
                .AddQuery(addressQuery)
                .Build();

            Snapshot.Match(query);
        }
    }
}