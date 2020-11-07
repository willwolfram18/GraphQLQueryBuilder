using System;
using System.Linq.Expressions;
using FluentAssertions;
using FluentAssertions.Execution;
using GraphQLQueryBuilder.Abstractions.Language;
using GraphQLQueryBuilder.Tests.Models;
using Moq;
using NUnit.Framework;
using static FluentAssertions.FluentActions;

namespace GraphQLQueryBuilder.Tests.QueryOperationBuilderTests
{
    public class When_Adding_An_Object_Collection_Field : QueryOperationBuilderTest
    {
        public When_Adding_An_Object_Collection_Field(GraphQLOperationType operationType) : base(operationType)
        {
        }
        
        [Test]
        public void If_Property_Expression_Is_Null_Then_ArgumentNullException_Is_Thrown()
        {
            var builder = CreateBuilderFor<SimpleSchema>();
            var fakeSelectionSet = Mock.Of<ISelectionSet<Customer>>();

            Action addingFieldWithoutAlias = () => builder.AddObjectCollectionField(null, fakeSelectionSet);
            Action addingFieldWithAlias = () => builder.AddObjectCollectionField("foo", null, fakeSelectionSet);

            using (new AssertionScope())
            {
                foreach (var addingField in new [] { addingFieldWithAlias, addingFieldWithoutAlias})
                {
                    Invoking(addingField).Should().ThrowExactly<ArgumentNullException>()
                        .Where(e => e.ParamName == "expression");
                }
            }
        }
        
        [Test]
        public void If_Selection_Set_Is_Null_Then_An_ArgumentNullException_Is_Thrown()
        {
            var builder = CreateBuilderFor<SimpleSchema>();

            Action addingFieldWithoutAlias = () => builder.AddObjectCollectionField(schema => schema.Customers, null);
            Action addingFieldWithAlias = () => builder.AddObjectCollectionField("foo", schema => schema.Customers, null);

            using (new AssertionScope())
            {
                foreach (var addingField in new [] { addingFieldWithAlias, addingFieldWithoutAlias})
                {
                    Invoking(addingField).Should().ThrowExactly<ArgumentNullException>()
                        .Where(e => e.ParamName == "selectionSet");
                }
            }
        }
        
        [Test]
        public void If_Expression_Is_Not_A_Member_Expression_Then_An_ArgumentException_Is_Thrown()
        {
            var builder = CreateBuilderFor<SimpleSchema>();
            var fakeSelectionSet = Mock.Of<ISelectionSet<string>>();

            Action addingFieldWithoutAlias = () => builder.AddObjectCollectionField(_ => new [] { "hello" }, fakeSelectionSet);
            Action addingFieldWithAlias = () => builder.AddObjectCollectionField("foo", _ => new [] { "hello" }, fakeSelectionSet);

            using (new AssertionScope())
            {
                foreach (var addingField in new [] { addingFieldWithAlias, addingFieldWithoutAlias})
                {
                    Invoking(addingField).Should().ThrowExactly<ArgumentException>()
                        .WithMessage($"A {nameof(MemberExpression)} was not provided.*")
                        .Where(e => e.ParamName == "expression");
                }
            }
        }
        
        [Test]
        public void If_Expression_Is_Not_A_Property_Of_The_Class_Then_An_InvalidOperationException_Is_Thrown()
        {
            var builder = SelectionSetBuilder.For<Customer>();
            var fakeSelectionSet = Mock.Of<ISelectionSet<PhoneNumber>>();

            Action addingFieldWithoutAlias = () =>
                builder.AddObjectCollectionField(schema => schema.CustomerContact.PhoneNumbers, fakeSelectionSet);
            Action addingFieldWithAlias = () =>
                builder.AddObjectCollectionField("foo", schema => schema.CustomerContact.PhoneNumbers, fakeSelectionSet);

            using (new AssertionScope())
            {
                foreach (var addingField in new [] { addingFieldWithAlias, addingFieldWithoutAlias})
                {
                    Invoking(addingField).Should().ThrowExactly<InvalidOperationException>()
                        .WithMessage($"Property '*' is not a member of type '{typeof(Customer).FullName}'.");
                }
            }
        }
        
        [InvalidAliasNamesTestCaseSource]
        public void If_Property_Alias_Is_Not_A_Valid_GraphQL_Name_Then_ArgumentException_Is_Thrown_Stating_Alias_Is_Not_Valid(
            string alias, string because)
        {
            var builder = CreateBuilderFor<SimpleSchema>();
            var fakeSelectionSet = Mock.Of<ISelectionSet<Customer>>();

            Action method = () => builder.AddObjectCollectionField(alias, schema => schema.Customers, fakeSelectionSet);

            Invoking(method).Should().ThrowExactly<ArgumentException>(because)
                .Where(e => e.ParamName == "alias", "because the alias parameter is the problem")
                .Where(e => e.HelpLink == "https://spec.graphql.org/June2018/#Name", "because this is the link to the GraphQL 'Name' spec");
        }
    }
}