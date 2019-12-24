using GraphQLQueryBuilder.Tests.Models;
using NUnit.Framework;
using System;
using Snapshooter.Json;

namespace GraphQLQueryBuilder.Tests
{
    public class OperationNameTests : TestClass
    {
        private const string DuplicateFragmentSkipReason = "Determine duplicate fragment resolution.";
        private const string DefaultOperationName = "CompleteAddressOperation";

        [Test]
        public void IfOperationNameIsAddedToQueryRootBuilderThenOperationNameIsInQueryDefinition()
        {
            var query = new QueryRootBuilder()
                .AddOperationName("ExampleOperation")
                .Build();

            Snapshot.Match(query, GetSnapshotName());
        }

        [Test]
        public void IfQueryRootWithOperationNameBuilderIsUsedThenOperationNameIsInTheQueryDefinition()
        {
            var query = new QueryRootWithOperationNameBuilder("NamedOperation")
                .Build();

            Snapshot.Match(query, GetSnapshotName());
        }

        [Test]
        public void IfQueryRootWithOperationNameBuilderIsUsedThenChildQueriesAppear()
        {
            var addressQuery = CreateCompleteAddressQueryWithoutFragment();

            var query = CreateQueryRootWithDefaultOperationName()
                .AddQuery(addressQuery)
                .Build();

            Snapshot.Match(query, GetSnapshotName());
        }

        [Test]
        public void IfQueryRootWithOperationNameBuilderIsUsedWithAliasedChildQueryThenAliasedChildQueryAppears()
        {
            var addressQuery = CreateCompleteAddressQueryWithoutFragment();

            var query = CreateQueryRootWithDefaultOperationName()
                .AddQuery("theAddress", addressQuery)
                .Build();

            Snapshot.Match(query, GetSnapshotName());
        }

        [Test]
        public void ThenFragmentDefinitionIsAddedToQuery()
        {
            var addressFragment = CreateCompleteAddressFragment();

            var query = new QueryRootWithOperationNameBuilder("EmptyQueryOperation")
                .AddFragment(addressFragment)
                .Build();

            Snapshot.Match(query, GetSnapshotName());
        }

        [Test]
        public void IfTheSameFragmentNameIsAddedTwiceThenInvalidOperationExceptionIsThrown()
        {
            var addressFragment = CreateCompleteAddressFragment();
            var customerFragment = new FragmentBuilder<Customer>(addressFragment.Name);

            var query = new QueryRootWithOperationNameBuilder(DefaultOperationName)
                .AddFragment(addressFragment);

            Assert.Throws<InvalidOperationException>(() => query.AddFragment(customerFragment));

                                    Snapshot.Match(query, GetSnapshotName());
        }

        [Test]
        public void IfSameFragmentWithDifferentNameIsAddedThenQueryIsBuilt()
        {
            var completeAddressFragment = CreateCompleteAddressFragment();
            var otherCompleteAddressFragment = CreateNamedCompleteAddressFragment("OtherFragment");

            var query = CreateQueryRootWithDefaultOperationName()
                .AddFragment(completeAddressFragment)
                .AddFragment(otherCompleteAddressFragment)
                .Build();

            Snapshot.Match(query, GetSnapshotName());
        }

        [Test]
        public void ThenFragmentDefinitionIsUsedInQuery()
        {
            var completeAddressFragment = CreateCompleteAddressFragment();
            var addressQuery = new QueryBuilder<Address>("address");

            addressQuery.AddFragment(completeAddressFragment);

            var query = CreateQueryRootWithDefaultOperationName()
                .AddQuery(addressQuery)
                .Build();

            Snapshot.Match(query, GetSnapshotName());
        }

        [Test]
        public void IfFragmentAndFieldsAreSpecifiedThenBothAreUsedInTheQuery()
        {
            var addressQuery = CreateCompleteAddressQueryWithoutFragment();
            var completeAddressFragment = CreateCompleteAddressFragment();

            addressQuery.AddFragment(completeAddressFragment);

            var query = CreateQueryRootWithDefaultOperationName()
                .AddQuery(addressQuery)
                .Build();

            Snapshot.Match(query, GetSnapshotName());
        }

        [Test]
        [Ignore(DuplicateFragmentSkipReason)]
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

        [Test]
        [Ignore(DuplicateFragmentSkipReason)]
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

        private static QueryRootWithOperationNameBuilder CreateQueryRootWithDefaultOperationName()
        {
            return new QueryRootWithOperationNameBuilder(DefaultOperationName);
        }

        private static QueryBuilder<Address> CreateCompleteAddressQueryWithoutFragment()
        {
            var addressQuery = new QueryBuilder<Address>("address");

            addressQuery = AddAllAddressPropertiesToQueryBuilder(addressQuery);

            return addressQuery;
        }

        private static QueryBuilder<Address> AddAllAddressPropertiesToQueryBuilder(QueryBuilder<Address> addressQuery)
        {
            addressQuery.AddField(address => address.Street1)
                .AddField(address => address.Street2)
                .AddField(address => address.City)
                .AddField(address => address.State)
                .AddField(address => address.ZipCode);

            return addressQuery;
        }

        private static FragmentBuilder<Address> CreateCompleteAddressFragment()
        {
            return CreateNamedCompleteAddressFragment("CompleteAddress");
        }

        private static FragmentBuilder<Address> CreateNamedCompleteAddressFragment(string name)
        {
            var addressFragment = new FragmentBuilder<Address>(name)
                .AddField(address => address.Street1)
                .AddField(address => address.Street2)
                .AddField(address => address.City)
                .AddField(address => address.State)
                .AddField(address => address.ZipCode);
            return addressFragment;
        }
    }
}
