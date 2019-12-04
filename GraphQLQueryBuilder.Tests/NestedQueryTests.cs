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
        public void IfAliasIsIncludedThenEmptyNestedQueryIsAliased()
        {
            var addressQuery = new QueryBuilder<Address>("address");
            var customerQuery = new QueryBuilder<Customer>("customer")
                .AddQuery("primaryAddress", addressQuery)
                .AddQuery("billingAddress", addressQuery);

            var query = new QueryRootBuilder()
                .AddQuery(customerQuery)
                .Build();

            Snapshot.Match(query);
        }

        [Fact]
        public void ThenPropertiesAreBeforeChildQueries()
        {
            var addressQuery = new QueryBuilder<Address>("address")
                .AddField(address => address.Street1)
                .AddField(address => address.Street2)
                .AddField(address => address.City)
                .AddField(address => address.State)
                .AddField(address => address.ZipCode);

            var customerQuery = new QueryBuilder<Customer>("customer")
                .AddQuery(addressQuery)
                .AddField(c => c.Id)
                .AddField(c => c.AccountNumber);

            var query = new QueryRootBuilder()
                .AddQuery(customerQuery)
                .Build();

            Snapshot.Match(query);
        }

        [Fact]
        public void ThenAddPropertyIncludesConfiguredQuery()
        {
            var customerQuery = new QueryBuilder<Customer>("customer")
                .AddField(c => c.Id)
                .AddField(c => c.AccountNumber)
                .AddField(
                    c => c.CustomerContact,
                    contactQuery => contactQuery.AddField(c => c.FirstName)
                        .AddField(c => c.LastName)
                );

            var query = new QueryRootBuilder()
                .AddQuery(customerQuery)
                .Build();

            Snapshot.Match(query);
        }

        [Fact]
        public void ThenAddPropertyIncludesAliasForConfiguredQuery()
        {
            var customerQuery = new QueryBuilder<Customer>("customer")
                .AddField(customer => customer.Id)
                .AddField(customer => customer.AccountNumber)
                .AddField(
                    "theContact",
                    c => c.CustomerContact,
                    contactQuery => contactQuery.AddField(contact => contact.FirstName)
                        .AddField(contact => contact.LastName)
                );
            var query = new QueryRootBuilder()
                .AddQuery(customerQuery)
                .Build();

            Snapshot.Match(query);
        }

        [Fact]
        public void ThenChildQueryOfChildQueryIsConfigured()
        {
            var customerQuery = new QueryBuilder<Customer>("customer")
                .AddField(c => c.Id)
                .AddField(c => c.AccountNumber)
                .AddField(
                    c => c.CustomerContact,
                    contactQuery => contactQuery
                        .AddField(contact => contact.FirstName)
                        .AddField(contact => contact.LastName)
                        .AddField(contact => contact.Address,
                            addressQuery => addressQuery
                                .AddField(address => address.Street1)
                                .AddField(address => address.Street2)
                                .AddField(address => address.City)
                                .AddField(address => address.State)
                                .AddField(address => address.ZipCode)
                        )
                );

            var query = new QueryRootBuilder()
                .AddQuery(customerQuery)
                .Build();

            Snapshot.Match(query);
        }

        [Fact]
        public void ThenChildCollectionPropertiesArePresent()
        {
            var customerQuery = new QueryBuilder<Customer>("customer")
                .AddField(c => c.Id)
                .AddField(
                    c => c.CustomerContact,
                    contactQuery => contactQuery.AddCollectionField(
                        contact => contact.PhoneNumbers,
                        phoneNumberQuery => phoneNumberQuery
                            .AddField(p => p.Number)
                            .AddField(p => p.Extension)
                    )
                );

            var query = new QueryRootBuilder()
                .AddQuery(customerQuery)
                .Build();

            Snapshot.Match(query);
        }
    }
}