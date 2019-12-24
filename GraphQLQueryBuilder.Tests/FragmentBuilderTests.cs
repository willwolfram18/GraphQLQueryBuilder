using GraphQLQueryBuilder.Tests.Models;
using NUnit.Framework;
using Snapshooter.Json;

namespace GraphQLQueryBuilder.Tests
{
    public class FragmentBuilderTests : TestClass
    {
        private const string CompleteAddressFragmentName = "CompleteAddress";

        [Test]
        public void IfNoFieldsSpecifiedThenEmptyFragmentIsCreated()
        {
            var fragment = new FragmentBuilder<Address>("EmptyAddress")
                .Build();

            Snapshot.Match(fragment, GetSnapshotName());
        }

        [Test]
        public void ThenOnlySpecifiedFieldsInFragment()
        {
            var fragment = new FragmentBuilder<Address>("PartialAddress")
                .AddField(address => address.Street1)
                .AddField(address => address.Street2)
                .AddField(address => address.ZipCode)
                .Build();

            Snapshot.Match(fragment, GetSnapshotName());
        }

        [Test]
        public void ThenFieldsIncludedInFragment()
        {
            var fragment = CreateCompleteAddressFragment()
                .Build();

            Snapshot.Match(fragment, GetSnapshotName());
        }

        [Test]
        public void ThenAddedFragmentIsIncludedInFragmentContent()
        {
            var contactFragment = CreatePartialContactFragment();
            var fragment = new FragmentBuilder<Customer>("PartialCustomer")
                .AddField(customer => customer.Id)
                .AddFragment(customer => customer.CustomerContact, contactFragment)
                .Build();

            Snapshot.Match(fragment, GetSnapshotName());
        }

        [Test]
        public void IfGrandchildFragmentIsIncludedInChildFragmentThenOnlyChildFragmentNameIsInFinalContent()
        {
            var addressFragment = CreateCompleteAddressFragment();
            var contactFragment = CreatePartialContactFragment()
                .AddFragment(contact => contact.Address, addressFragment);
            var fragment = new FragmentBuilder<Customer>("PartialCustomer")
                .AddField(customer => customer.Id)
                .AddFragment(customer => customer.CustomerContact, contactFragment)
                .Build();

            Snapshot.Match(fragment, GetSnapshotName());
        }

        [Test]
        [Ignore(DuplicateFragmentSkipReason)]
        public void IfSameFragmentReferenceIsAddedTwiceThenTODO()
        {
            var addressFragment = CreateCompleteAddressFragment();
            var fragment = new FragmentBuilder<Contact>("PartialCustomer")
                .AddFragment(contact => contact.Address, addressFragment)
                .AddFragment(contact => contact.Address, addressFragment)
                .Build();

            Assert.True(false, DuplicateFragmentSkipReason);
        }

        [Test]
        [Ignore(DuplicateFragmentSkipReason)]
        public void IfIdenticalFragmentIsAddedTwiceThenTODO()
        {
            var completeAddressFragment = CreateCompleteAddressFragment();
            var duplicateAddressFragment = CreateCompleteAddressFragment();
            var fragment = new FragmentBuilder<Contact>("PartialCustomer")
                .AddFragment(contact => contact.Address, completeAddressFragment)
                .AddFragment(contact => contact.Address, duplicateAddressFragment)
                .Build();

            Assert.True(false, DuplicateFragmentSkipReason);
        }

        [Test]
        [Ignore(DuplicateFragmentSkipReason)]
        public void IfDifferentFragmentWithIdenticalNameIsAddedTwiceThenTODO()
        {
            var completeAddressFragment = CreateCompleteAddressFragment();
            var fragmentWithSameName = new FragmentBuilder<Address>(completeAddressFragment.Name)
                .AddField(address => address.Street1);
            var fragment = new FragmentBuilder<Contact>("PartialCustomer")
                .AddFragment(contact => contact.Address, completeAddressFragment)
                .AddFragment(contact => contact.Address, fragmentWithSameName)
                .Build();

            Assert.True(false, DuplicateFragmentSkipReason);
        }

        private FragmentBuilder<Address> CreateCompleteAddressFragment()
        {
            return new FragmentBuilder<Address>(CompleteAddressFragmentName)
                .AddField(address => address.Street1)
                .AddField(address => address.Street2)
                .AddField(address => address.City)
                .AddField(address => address.State)
                .AddField(address => address.ZipCode);
        }

        private FragmentBuilder<Contact> CreatePartialContactFragment()
        {
            return new FragmentBuilder<Contact>("PartialContact")
                .AddField(contact => contact.FirstName)
                .AddField(contact => contact.LastName);
        }
    }
}
