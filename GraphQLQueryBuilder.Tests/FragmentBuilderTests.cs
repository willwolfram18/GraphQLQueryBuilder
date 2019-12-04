using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQLQueryBuilder.Tests.Models;
using Snapshooter.Xunit;
using Xunit;

namespace GraphQLQueryBuilder.Tests
{
    public class FragmentBuilderTests
    {
        [Fact]
        public void IfNoFieldsSpecifiedThenEmptyFragmentIsCreated()
        {
            var fragment = new FragmentBuilder<Address>("EmptyAddress")
                .Build();

            Snapshot.Match(fragment);
        }

        [Fact]
        public void ThenOnlySpecifiedFieldsInFragment()
        {
            var fragment = new FragmentBuilder<Address>("PartialAddress")
                .AddField(address => address.Street1)
                .AddField(address => address.Street2)
                .AddField(address => address.ZipCode)
                .Build();

            Snapshot.Match(fragment);
        }

        [Fact]
        public void ThenFieldsIncludedInFragment()
        {
            var fragment = new FragmentBuilder<Address>("CompleteAddress")
                .AddField(address => address.Street1)
                .AddField(address => address.Street2)
                .AddField(address => address.City)
                .AddField(address => address.State)
                .AddField(address => address.ZipCode)
                .Build();

            Snapshot.Match(fragment);
        }
    }
}
