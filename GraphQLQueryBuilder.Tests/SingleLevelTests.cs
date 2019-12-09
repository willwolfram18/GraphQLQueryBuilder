using System;
using GraphQLQueryBuilder.Tests.Models;
using Snapshooter.Xunit;
using Xunit;

namespace GraphQLQueryBuilder.Tests
{
    public class SingleLevelTests
    {
        private const string DuplicateFragmentSkipReason = "Determine duplicate fragment resolution.";
        private QueryBuilder<Address> _addressQuery;

        public SingleLevelTests()
        {
            _addressQuery = AddressQueries.CreateCompleteAddressQueryWithoutFragment();
        }

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
            var query = new QueryRootBuilder()
                .AddQuery(_addressQuery)
                .Build();

            Snapshot.Match(query);
        }

        [Fact]
        public void SimpleAddressQueryWithAlias()
        {
            var query = new QueryRootBuilder()
                .AddQuery("primaryAddress", _addressQuery)
                .Build();

            Snapshot.Match(query);
        }

        [Fact]
        public void RepeatedPropertiesIncludedInQuery()
        {
            _addressQuery = AddressQueries.AddAllAddressPropertiesToQueryBuilder(_addressQuery);

            var query = new QueryRootBuilder()
                .AddQuery(_addressQuery)
                .Build();

            Snapshot.Match(query);
        }

        [Fact]
        public void ThenFragmentDefinitionIsAddedToQuery()
        {
            var addressFragment = AddressQueries.CreateCompleteAddressFragment();

            var query = new QueryRootBuilder()
                .AddFragment(addressFragment)
                .Build();

            Snapshot.Match(query);
        }

        [Fact]
        public void IfTheSameFragmentNameIsAddedTwiceThenInvalidOperationExceptionIsThrown()
        {
            var addressFragment = AddressQueries.CreateCompleteAddressFragment();
            var customerFragment = new FragmentBuilder<Customer>(addressFragment.Name);

            var query = new QueryRootBuilder()
                .AddFragment(addressFragment);

            Assert.Throws<InvalidOperationException>(() => query.AddFragment(customerFragment));

            // TODO check error message
        }

        [Fact]
        public void IfSameFragmentWithDifferentNameIsAddedThenQueryIsBuilt()
        {
            var completeAddressFragment = AddressQueries.CreateCompleteAddressFragment();
            var otherCompleteAddressFragment = AddressQueries.CreateNamedCompleteAddressFragment("OtherFragment");

            var query = new QueryRootBuilder()
                .AddFragment(completeAddressFragment)
                .AddFragment(otherCompleteAddressFragment)
                .Build();

            Snapshot.Match(query);
        }

        [Fact]
        public void ThenFragmentDefinitionIsUsedInQuery()
        {
            var completeAddressFragment = AddressQueries.CreateCompleteAddressFragment();
            var addressQuery = new QueryBuilder<Address>("address");

            addressQuery.AddFragment(completeAddressFragment);

            var query = new QueryRootBuilder()
                .AddQuery(addressQuery)
                .Build();

            Snapshot.Match(query);
        }

        [Fact]
        public void IfFragmentAndFieldsAreSpecifiedThenBothAreUsedInTheQuery()
        {
            var addressQuery = AddressQueries.CreateCompleteAddressQueryWithoutFragment();
            var completeAddressFragment = AddressQueries.CreateCompleteAddressFragment();

            addressQuery.AddFragment(completeAddressFragment);

            var query = new QueryRootBuilder()
                .AddQuery(addressQuery)
                .Build();

            Snapshot.Match(query);
        }

        [Fact(Skip = DuplicateFragmentSkipReason)]
        public void IfIdenticalFragmentIsAddedToQueryAndQueryRootThenTODO()
        {
            var queryAddressFragment = AddressQueries.CreateCompleteAddressFragment();
            var queryRootAddressFragment = AddressQueries.CreateCompleteAddressFragment();

            var addressQuery = new QueryBuilder<Address>("address")
                .AddFragment(queryAddressFragment);

            var query = new QueryRootBuilder()
                .AddFragment(queryRootAddressFragment)
                .AddQuery(addressQuery)
                .Build();

            Assert.False(true, DuplicateFragmentSkipReason);
        }

        [Fact(Skip = DuplicateFragmentSkipReason)]
        public void IfDifferentFragmentsWithIdenticalNamesAreAddedToQueryAndQueryRootThenTODO()
        {
            var completeAddressFragment = AddressQueries.CreateCompleteAddressFragment();
            var incompleteAddressFragment = new FragmentBuilder<Address>(completeAddressFragment.Name);

            var addressQuery = new QueryBuilder<Address>("address")
                .AddFragment(completeAddressFragment);

            var query = new QueryRootBuilder()
                .AddFragment(incompleteAddressFragment)
                .AddQuery(addressQuery)
                .Build();

            Assert.False(true, DuplicateFragmentSkipReason);
        }
    }
}