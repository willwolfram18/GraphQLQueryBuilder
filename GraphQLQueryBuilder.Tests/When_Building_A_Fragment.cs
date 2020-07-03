using GraphQLQueryBuilder.Tests.Models;
using NUnit.Framework;

namespace GraphQLQueryBuilder.Tests
{
    public class When_Building_A_Fragment : TestClass
    {
        private const string CompleteAddressFragmentName = "CompleteAddress";

        [Test]
        public void If_Nothing_Is_Added_To_The_Fragment_Then_The_Fragment_Content_Is_Empty()
        {
            Assert.Fail("TODO");
        }

        [Test]
        public void Then_Only_The_Added_Fields_Are_In_The_Fragment_Content()
        {
            Assert.Fail("TODO");
        }

        [Test]
        public void If_All_Fields_Added_To_Fragment_Then_Fragment_Includes_All_Fields()
        {
            Assert.Fail("TODO");
        }

        [Test]
        public void Then_Added_Fragment_Is_Included_In_Specified_Field()
        {
            Assert.Fail("TODO");
        }

        [Test]
        public void If_Grandchild_Fragment_Is_Included_In_Child_Fragment_Then_Only_The_Child_Fragment_Name_Is_In_The_Fragment_Content()
        {
            Assert.Fail("TODO");
        }

        [Test]
        [Ignore(DuplicateFragmentSkipReason)]
        public void IfSameFragmentReferenceIsAddedTwiceThenTODO()
        {
            Assert.Fail("TODO");
        }

        [Test]
        [Ignore(DuplicateFragmentSkipReason)]
        public void IfIdenticalFragmentIsAddedTwiceThenTODO()
        {
            Assert.Fail("TODO");
        }

        [Test]
        [Ignore(DuplicateFragmentSkipReason)]
        public void IfDifferentFragmentWithIdenticalNameIsAddedTwiceThenTODO()
        {
            Assert.Fail("TODO");
        }
    }
}
