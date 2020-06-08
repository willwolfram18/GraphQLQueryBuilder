using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;

namespace GraphQLQueryBuilder.Tests
{
    public class When_Building_A_Query : TestClass
    {
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

            var queryBuilder = new QueryOperationBuilder(expectedOperationName);

            queryBuilder.OperationName.Should().Be(expectedOperationName);

            QueryContentShouldMatchSnapshotForTest(queryBuilder);
        }

        [BadGraphQLNamesTestCaseSource]
        public void Then_Operation_Name_Adheres_To_GraphQL_Spec(string badOperationName, string becauseReason)
        {
            Action constructor = () => _ = new QueryOperationBuilder(badOperationName);

            constructor.Should().Throw<ArgumentException>(becauseReason);
        }

        [Test]
        public void Then_Query_Operation_Cannot_Be_Added_As_A_Selection_Set([Values] GraphQLOperationTypes operationType)
        {
            var queryBuilder = new QueryOperationBuilder();

            Action addingOperation = () => queryBuilder.AddField("bad", new FakeGraphQLOperation(operationType));

            addingOperation.Should().Throw<InvalidOperationException>("because you cannot add a query operation as a selection set");

            QueryContentShouldMatchSnapshotForTest(queryBuilder);
        }

        [Test]
        public void Then_Added_Fields_Are_Included_In_Query()
        {
            var query = new QueryOperationBuilder()
                .AddField("field1")
                .AddField("field2")
                .Build();

            ResultMatchesSnapshotOfMatchingClassAndTestName(query);
        }

        [BadGraphQLNamesTestCaseSource]
        public void Then_Added_Fields_Must_Adhere_To_GraphQL_Spec(string badFieldName, string becauseReason)
        {
            var queryBuilder = new QueryOperationBuilder();

            Action addingIllegalName = () => queryBuilder.AddField(badFieldName);

            addingIllegalName.Should().Throw<ArgumentException>(becauseReason);

            QueryContentShouldMatchSnapshotForTest(queryBuilder);
        }

        [Test]
        public void Then_Aliases_For_Added_Fields_Are_Included_In_Query()
        {
            var query = new QueryOperationBuilder()
                .AddField("aliasA", "field1")
                .AddField("aliasB", "field2")
                .Build();

            ResultMatchesSnapshotOfMatchingClassAndTestName(query);
        }

        [BadGraphQLNamesTestCaseSource]
        public void Then_Aliases_For_Added_Fields_Must_Adhere_To_GraphQL_Spec(string badAliasName, string becauseReason)
        {
            var queryBuilder = new QueryOperationBuilder();

            Action addingIllegalAlias = () => queryBuilder.AddField(badAliasName, "field1");

            addingIllegalAlias.Should().Throw<ArgumentException>(becauseReason);

            QueryContentShouldMatchSnapshotForTest(queryBuilder);
        }

        [Test]
        public void Then_Selection_Set_For_Fields_Cannot_Be_Null()
        {
            var queryBuilder = new QueryOperationBuilder();

            FluentActions.Invoking(() => queryBuilder.AddField("field1", selectionSet: null)).Should().Throw<ArgumentNullException>();
            FluentActions.Invoking(() => queryBuilder.AddField("aliasA", "field1", null)).Should().Throw<ArgumentNullException>();

            QueryContentShouldMatchSnapshotForTest(queryBuilder);
        }

        [BadGraphQLNamesTestCaseSource]
        public void Then_Alias_For_Selection_Set_Field_Must_Adhere_To_GraphQL_Spec(string badAliasName, string becauseReason)
        {
            var queryBuilder = new QueryOperationBuilder();
            var fakeSelectionSet = Mock.Of<ISelectionSet>();

            Action addingSelectionSetFieldWithBadAlias = () => queryBuilder.AddField(badAliasName, "field1", fakeSelectionSet);

            addingSelectionSetFieldWithBadAlias.Should().Throw<ArgumentException>(becauseReason);

            QueryContentShouldMatchSnapshotForTest(queryBuilder);
        }

        [BadGraphQLNamesTestCaseSource]
        public void Then_Field_Name_For_Selection_Set_Must_Adhere_To_GraphQL_Spec(string badFieldName, string becauseReason)
        {
            var queryBuilder = new QueryOperationBuilder();
            var fakeSelectionSet = Mock.Of<ISelectionSet>();

            FluentActions.Invoking(() => queryBuilder.AddField(badFieldName, fakeSelectionSet)).Should().Throw<ArgumentException>(becauseReason);
            FluentActions.Invoking(() => queryBuilder.AddField("aliasA", badFieldName, fakeSelectionSet)).Should().Throw<ArgumentException>(becauseReason);

            QueryContentShouldMatchSnapshotForTest(queryBuilder);
        }

        [Test]
        public void Then_Selection_Set_Is_Included_In_Query_Content()
        {
            var fakeSelectionSet = Mock.Of<ISelectionSet>(selectionSet => selectionSet.Build() == "{ id, name }");
            var query = new QueryOperationBuilder()
                .AddField("field1", fakeSelectionSet)
                .AddField("aliasB", "field2", fakeSelectionSet)
                .Build();

            ResultMatchesSnapshotOfMatchingClassAndTestName(query);
        }

        [Test]
        public void If_Fragment_Is_Added_As_Selection_Set_Then_Fragment_Spread_And_Definition_Is_Rendered_In_Query_Content()
        {
            var fakeFragment = Mock.Of<IFragmentContentBuilder>(fragment =>
                fragment.Name == "FakeFragment" &&
                fragment.Build() == "fragment FakeFragment on Thing { id, name }"
            );

            var query = new QueryOperationBuilder()
                .AddField("field1", fakeFragment)
                .AddField("aliasB", "field2", fakeFragment)
                .Build();

            ResultMatchesSnapshotOfMatchingClassAndTestName(query);
        }



        [Test]
        [Ignore("Still uses queryrootbuilder")]
        public void If_A_Fragment_Is_Added_To_The_Query_Root_Then_The_Fragment_Definition_Is_Included_In_The_Query_Content()
        {
            Assert.Fail();
        }

        [Test]
        [Ignore("Still uses queryrootbuilder")]
        public void If_The_Same_Query_Fragment_Name_Is_Added_Twice_Then_An_InvalidOperationException_Is_Thrown()
        {
            Assert.Fail();
        }

        [Test]
        [Ignore("Still uses queryrootbuilder")]
        public void If_The_Same_Fragment_With_A_Different_Name_Is_Added_To_Query_Root_Then_Both_Fragments_Are_In_The_Query_Content()
        {
            Assert.Fail();
        }

        [Test]
        [Ignore("Still uses queryrootbuilder")]
        public void If_A_Fragment_Is_Used_In_A_Query_Then_The_Fragment_Is_Part_Of_The_Query_Content()
        {
            Assert.Fail();
        }

        [Test]
        [Ignore("Still uses queryrootbuilder")]
        public void If_Fragment_And_Fields_Are_Added_To_A_Query_Then_Both_Are_Used_In_The_Query_Content()
        {
            Assert.Fail();
        }

        [Test]
        [Ignore(DuplicateFragmentSkipReason)]
        public void IfIdenticalFragmentIsAddedToQueryAndQueryRootThenTODO()
        {
            Assert.Fail(DuplicateFragmentSkipReason);
        }

        [Test]
        [Ignore(DuplicateFragmentSkipReason)]
        public void IfDifferentFragmentsWithIdenticalNamesAreAddedToQueryAndQueryRootThenTODO()
        {
            Assert.Fail(DuplicateFragmentSkipReason);
        }

        public class FakeGraphQLOperation : IGraphQLQueryContentBuilder, IQueryOperation
        {
            public FakeGraphQLOperation(GraphQLOperationTypes operationType)
            {
                OperationType = operationType;
            }

            public GraphQLOperationTypes OperationType { get; }

            public IGraphQLQueryContentBuilder AddField(string field)
            {
                throw new NotImplementedException();
            }

            public IGraphQLQueryContentBuilder AddField(string alias, string field)
            {
                throw new NotImplementedException();
            }

            public IGraphQLQueryContentBuilder AddField(string field, ISelectionSet selectionSet)
            {
                throw new NotImplementedException();
            }

            public IGraphQLQueryContentBuilder AddField(string alias, string field, ISelectionSet selectionSet)
            {
                throw new NotImplementedException();
            }

            public IGraphQLQueryContentBuilder AddFragment(IFragmentContentBuilder fragment)
            {
                throw new NotImplementedException();
            }

            public string Build()
            {
                throw new NotImplementedException();
            }
        }
    }
}