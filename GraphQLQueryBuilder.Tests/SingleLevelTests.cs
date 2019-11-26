using GraphQLQueryBuilder.Tests.Models;
using Snapshooter.Xunit;
using Xunit;

namespace GraphQLQueryBuilder.Tests
{
    public class Test
    {
        [Fact]
        public void SimpleAddressQuery()
        {
            var query = new QueryBuilder<Address>()
                .AddQuery("address")
                .AddProperty(address => address.Street1)
                .AddProperty(address => address.Street2)
                .AddProperty(address => address.City)
                .AddProperty(address => address.State)
                .AddProperty(address => address.ZipCode)
                .Build();

            Snapshot.Match(query);
        }

        [Fact]
        public void RepeatedPropertiesIncludedInQuery()
        {
            var query = new QueryBuilder<Address>()
                .AddQuery("address")
                .AddProperty(address => address.Street1)
                .AddProperty(address => address.Street1)
                .AddProperty(address => address.Street1)
                .AddProperty(address => address.Street2)
                .AddProperty(address => address.City)
                .AddProperty(address => address.State)
                .AddProperty(address => address.ZipCode)
                .Build();

            Snapshot.Match(query);
        }
    }
}