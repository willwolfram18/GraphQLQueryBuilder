using GraphQLQueryBuilder.Tests.Models;
using NUnit.Framework;
using System;
using Snapshooter.Json;

namespace GraphQLQueryBuilder.Tests
{
    public class When_Building_A_Query_Root_With_An_Operation_Name : TestClass
    {
        private const string DefaultOperationName = "CompleteAddressOperation";

        [Test]
        public void If_Operation_Name_Is_Added_To_A_Query_Root_Builder_Then_The_Operation_Name_Is_In_The_Query_Content()
        {
            var query = new QueryRootBuilder()
                .AddOperationName("ExampleOperation")
                .Build();

            ResultMatchesSnapshotOfMatchingClassAndTestName(query);
        }

        [Test]
        public void If_Query_Root_Builder_With_Operation_Name_Is_Created_Then_Operation_Name_Is_In_The_Query_Content()
        {
            var query = new QueryRootWithOperationNameBuilder("NamedOperation")
                .Build();

            ResultMatchesSnapshotOfMatchingClassAndTestName(query);
        }

        [Test]
        public void If_A_Query_Is_Added_Then_The_Added_Query_Is_Included_In_The_Content()
        {
            var addressQuery = CreateCompleteAddressQueryWithoutFragment();

            var query = CreateQueryRootWithDefaultOperationName()
                .AddQuery(addressQuery)
                .Build();

            ResultMatchesSnapshotOfMatchingClassAndTestName(query);
        }

        [Test]
        public void If_A_Query_Is_Added_With_An_Alias_The_Aliased_Query_Is_Included_In_The_Content()
        {
            var addressQuery = CreateCompleteAddressQueryWithoutFragment();

            var query = CreateQueryRootWithDefaultOperationName()
                .AddQuery("theAddress", addressQuery)
                .Build();

            ResultMatchesSnapshotOfMatchingClassAndTestName(query);
        }

        [Test]
        public void If_Fragment_Is_Added_To_Query_Root_Then_Fragment_Definition_Is_Included_In_Query_Content()
        {
            var addressFragment = CreateCompleteAddressFragment();

            var query = new QueryRootWithOperationNameBuilder("EmptyQueryOperation")
                .AddFragment(addressFragment)
                .Build();

            ResultMatchesSnapshotOfMatchingClassAndTestName(query);
        }

        [Test]
        public void If_Fragment_With_The_Same_Name_Is_Added_Twice_Then_An_InvalidOperationException_Is_Thrown()
        {
            var addressFragment = CreateCompleteAddressFragment();
            var customerFragment = new FragmentBuilder<Customer>(addressFragment.Name);

            var query = new QueryRootWithOperationNameBuilder(DefaultOperationName)
                .AddFragment(addressFragment);

            Assert.Throws<InvalidOperationException>(() => query.AddFragment(customerFragment));
        }

        [Test]
        public void If_The_Same_Fragment_With_A_Different_Name_Is_Added_To_The_Query_Then_Both_Fragments_Are_In_The_Query_Content()
        {
            var completeAddressFragment = CreateCompleteAddressFragment();
            var otherCompleteAddressFragment = CreateNamedCompleteAddressFragment("OtherFragment");

            var query = CreateQueryRootWithDefaultOperationName()
                .AddFragment(completeAddressFragment)
                .AddFragment(otherCompleteAddressFragment)
                .Build();

            ResultMatchesSnapshotOfMatchingClassAndTestName(query);
        }

        [Test]
        public void If_Query_Uses_A_Fragment_Then_The_Fragment_Is_Used_In_The_Query()
        {
            var completeAddressFragment = CreateCompleteAddressFragment();
            var addressQuery = new QueryBuilder<Address>("address");

            addressQuery.AddFragment(completeAddressFragment);

            var query = CreateQueryRootWithDefaultOperationName()
                .AddQuery(addressQuery)
                .Build();

            ResultMatchesSnapshotOfMatchingClassAndTestName(query);
        }

        [Test]
        public void If_A_Query_Includes_Both_A_Fragment_And_FieldsThen_Both_Are_Used_In_The_Query_Content()
        {
            var addressQuery = CreateCompleteAddressQueryWithoutFragment();
            var completeAddressFragment = CreateCompleteAddressFragment();

            addressQuery.AddFragment(completeAddressFragment);

            var query = CreateQueryRootWithDefaultOperationName()
                .AddQuery(addressQuery)
                .Build();

            ResultMatchesSnapshotOfMatchingClassAndTestName(query);
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
