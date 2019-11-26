using GraphQLQueryBuilder.Tests.Models;
using Snapshooter.Xunit;
using Xunit;

namespace GraphQLQueryBuilder.Tests
{
    public class NestedQueryTests
    {
        [Fact]
        public void ThenNestedQueryIsEmpty()
        {
            var addressQuery = new QueryBuilder<Address>("address");
            var customerQuery = new QueryBuilder<Customer>("customer")
                .AddQuery(addressQuery);

            var query = new QueryRootBuilder()
                .AddQuery(customerQuery)
                .Build();

            Snapshot.Match(query);
        }

        [Fact]
        public void ThenPropertiesAreBeforeChildQueries()
        {
            var addressQuery = new QueryBuilder<Address>("address")
                .AddProperty(address => address.Street1)
                .AddProperty(address => address.Street2)
                .AddProperty(address => address.City)
                .AddProperty(address => address.State)
                .AddProperty(address => address.ZipCode);

            var customerQuery = new QueryBuilder<Customer>("customer")
                .AddQuery(addressQuery)
                .AddProperty(c => c.Id)
                .AddProperty(c => c.AccountNumber);

            var query = new QueryRootBuilder()
                .AddQuery(customerQuery)
                .Build();

            Snapshot.Match(query);
        }

        [Fact]
        public void ThenAddPropertyIncludesConfiguredQuery()
        {
            var customerQuery = new QueryBuilder<Customer>("customer")
                .AddProperty(c => c.Id)
                .AddProperty(c => c.AccountNumber)
                .AddProperty(
                    c => c.CustomerContact,
                    contactQuery => contactQuery.AddProperty(c => c.FirstName)
                        .AddProperty(c => c.LastName)
                );

            var query = new QueryRootBuilder()
                .AddQuery(customerQuery)
                .Build();

            Snapshot.Match(query);
        }
    }
}