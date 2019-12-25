using GraphQLQueryBuilder.Tests.Models;
using NUnit.Framework;
using Snapshooter.Json;

namespace GraphQLQueryBuilder.Tests
{
    public class When_Building_A_Fragment : TestClass
    {
        private const string CompleteAddressFragmentName = "CompleteAddress";

        [Test]
        public void If_Nothing_Is_Added_To_The_Fragment_Then_The_Fragment_Content_Is_Empty()
        {
            var fragment = new FragmentBuilder<Address>("EmptyAddress")
                .Build();

            Snapshot.Match(fragment, GetSnapshotName());
        }

        [Test]
        public void Then_Only_The_Added_Fields_Are_In_The_Fragment_Content()
        {
            var fragment = new FragmentBuilder<Address>("PartialAddress")
                .AddField(address => address.Street1)
                .AddField(address => address.Street2)
                .AddField(address => address.ZipCode)
                .Build();

            Snapshot.Match(fragment, GetSnapshotName());
        }

        [Test]
        public void If_All_Fields_Added_To_Fragment_Then_Fragment_Includes_All_Fields()
        {
            var fragment = CreateCompleteAddressFragment()
                .Build();

            Snapshot.Match(fragment, GetSnapshotName());
        }

        [Test]
        public void Then_Added_Fragment_Is_Included_In_Specified_Field()
        {
            var contactFragment = CreatePartialContactFragment();
            var fragment = new FragmentBuilder<Customer>("PartialCustomer")
                .AddField(customer => customer.Id)
                .AddFragment(customer => customer.CustomerContact, contactFragment)
                .Build();

            Snapshot.Match(fragment, GetSnapshotName());
        }

        [Test]
        public void If_Grandchild_Fragment_Is_Included_In_Child_Fragment_Then_Only_The_Child_Fragment_Name_Is_In_The_Fragment_Content()
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
