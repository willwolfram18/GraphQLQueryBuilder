using GraphQLQueryBuilder.Tests.Models;
using NUnit.Framework;
using Snapshooter.Json;
using System;

namespace GraphQLQueryBuilder.Tests
{
    public class When_Building_A_Query : TestClass
    {
        private QueryBuilder<Address> _addressQuery;

        [SetUp]
        public void SetUp()
        {
            _addressQuery = AddressQueries.CreateCompleteAddressQueryWithoutFragment();
        }

        [Test]
        public void If_Nothing_Is_Added_To_The_Query_Then_An_Empty_Query_Is_Built()
        {
            var query = new QueryOperationBuilder()
                .Build();

            ResultMatchesSnapshotOfMatchingClassAndTestName(query);
        }

        [Test]
        public void Then_The_Operation_Name_Is_Included_In_The_Query_Content()
        {
            const string expectedOperationName = "ThisIsTheOperationName";

            var query = new QueryOperationBuilder(expectedOperationName)
                .Build();

            ResultMatchesSnapshotOfMatchingClassAndTestName(query);
        }

        [TestCase("todo")]
        public void Then_Operation_Name_Adheres_To_GraphQL_Spec(string badOperationNames)
        {
            Assert.Fail("Not implemented");
        }

        [Test]
        public void If_A_Query_Is_Added_To_The_Query_Root_Then_The_Query_Content_Includes_The_Child_Query_Content()
        {
            var query = new QueryRootBuilder()
                .AddQuery(_addressQuery)
                .Build();

            Snapshot.Match(query, GenerateSnapshotNameFromClassAndTestNames());
        }

        [Test]
        public void If_A_Query_Is_Added_To_The_Query_Root_With_An_Alias_Then_The_Query_Content_Aliases_The_Child_Query()
        {
            var query = new QueryRootBuilder()
                .AddQuery("primaryAddress", _addressQuery)
                .Build();

            Snapshot.Match(query, GenerateSnapshotNameFromClassAndTestNames());
        }

        [Test]
        public void If_Query_Has_The_Same_Property_Added_Multiple_Times_Then_Repeated_Properties_Are_Included_In_The_Query_Content()
        {
            _addressQuery = AddressQueries.AddAllAddressPropertiesToQueryBuilder(_addressQuery);

            var query = new QueryRootBuilder()
                .AddQuery(_addressQuery)
                .Build();

            Snapshot.Match(query, GenerateSnapshotNameFromClassAndTestNames());
        }

        [Test]
        public void If_A_Fragment_Is_Added_To_The_Query_Root_Then_The_Fragment_Definition_Is_Included_In_The_Query_Content()
        {
            var addressFragment = AddressQueries.CreateCompleteAddressFragment();

            var query = new QueryRootBuilder()
                .AddFragment(addressFragment)
                .Build();

            Snapshot.Match(query, GenerateSnapshotNameFromClassAndTestNames());
        }

        [Test]
        public void If_The_Same_Query_Fragment_Name_Is_Added_Twice_Then_An_InvalidOperationException_Is_Thrown()
        {
            var addressFragment = AddressQueries.CreateCompleteAddressFragment();
            var customerFragment = new FragmentBuilder<Customer>(addressFragment.Name);

            var query = new QueryRootBuilder()
                .AddFragment(addressFragment);

            Assert.Throws<InvalidOperationException>(() => query.AddFragment(customerFragment));

            Snapshot.Match(query, GenerateSnapshotNameFromClassAndTestNames());
        }

        [Test]
        public void If_The_Same_Fragment_With_A_Different_Name_Is_Added_To_Query_Root_Then_Both_Fragments_Are_In_The_Query_Content()
        {
            var completeAddressFragment = AddressQueries.CreateCompleteAddressFragment();
            var otherCompleteAddressFragment = AddressQueries.CreateNamedCompleteAddressFragment("OtherFragment");

            var query = new QueryRootBuilder()
                .AddFragment(completeAddressFragment)
                .AddFragment(otherCompleteAddressFragment)
                .Build();

            Snapshot.Match(query, GenerateSnapshotNameFromClassAndTestNames());
        }

        [Test]
        public void If_A_Fragment_Is_Used_In_A_Query_Then_The_Fragment_Is_Part_Of_The_Query_Content()
        {
            var completeAddressFragment = AddressQueries.CreateCompleteAddressFragment();
            var addressQuery = new QueryBuilder<Address>("address");

            addressQuery.AddFragment(completeAddressFragment);

            var query = new QueryRootBuilder()
                .AddQuery(addressQuery)
                .Build();

            Snapshot.Match(query, GenerateSnapshotNameFromClassAndTestNames());
        }

        [Test]
        public void If_Fragment_And_Fields_Are_Added_To_A_Query_Then_Both_Are_Used_In_The_Query_Content()
        {
            var addressQuery = AddressQueries.CreateCompleteAddressQueryWithoutFragment();
            var completeAddressFragment = AddressQueries.CreateCompleteAddressFragment();

            addressQuery.AddFragment(completeAddressFragment);

            var query = new QueryRootBuilder()
                .AddQuery(addressQuery)
                .Build();

            Snapshot.Match(query, GenerateSnapshotNameFromClassAndTestNames());
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
    }
}