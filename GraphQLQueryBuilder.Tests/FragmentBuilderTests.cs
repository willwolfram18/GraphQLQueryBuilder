using GraphQLQueryBuilder.Tests.Models;
using NUnit.Framework;
using Snapshooter.Json;

namespace GraphQLQueryBuilder.Tests
{
    public class FragmentBuilderTests : TestClass
    {
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
            var fragment = new FragmentBuilder<Address>("CompleteAddress")
                .AddField(address => address.Street1)
                .AddField(address => address.Street2)
                .AddField(address => address.City)
                .AddField(address => address.State)
                .AddField(address => address.ZipCode)
                .Build();

            Snapshot.Match(fragment, GetSnapshotName());
        }
    }
}
