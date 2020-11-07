using System;
using System.Linq.Expressions;
using FluentAssertions;
using FluentAssertions.Execution;
using GraphQLQueryBuilder.Abstractions.Language;
using GraphQLQueryBuilder.Tests.Models;
using Moq;
using NUnit.Framework;
using static FluentAssertions.FluentActions;

namespace GraphQLQueryBuilder.Tests.SelectionSetBuilderTests
{
    public class When_Adding_A_Collection_Field
    {
        [Test]
        public void If_Property_Expression_Is_Null_Then_ArgumentNullException_Is_Thrown()
        {
            var builder = SelectionSetBuilder.For<Contact>();
            var fakeSelectionSet = Mock.Of<ISelectionSet<PhoneNumber>>();

            Action addingFieldWithoutAlias = () => builder.AddCollectionField(null, fakeSelectionSet);
            Action addingFieldWithAlias = () => builder.AddCollectionField("foo", null, fakeSelectionSet);

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
            var builder = SelectionSetBuilder.For<Contact>();

            Action addingFieldWithoutAlias = () => builder.AddCollectionField(contact => contact.PhoneNumbers, null);
            Action addingFieldWithAlias = () => builder.AddCollectionField("foo", contact => contact.PhoneNumbers, null);

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
            var builder = SelectionSetBuilder.For<Contact>();
            var fakeSelectionSet = Mock.Of<ISelectionSet<string>>();

            Action addingFieldWithoutAlias = () => builder.AddCollectionField(_ => new [] { "hello" }, fakeSelectionSet);
            Action addingFieldWithAlias = () => builder.AddCollectionField("foo", _ => new [] { "hello" }, fakeSelectionSet);

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
                builder.AddCollectionField(customer => customer.CustomerContact.PhoneNumbers, fakeSelectionSet);
            Action addingFieldWithAlias = () =>
                builder.AddCollectionField("foo", customer => customer.CustomerContact.PhoneNumbers, fakeSelectionSet);

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
            var builder = SelectionSetBuilder.For<Contact>();
            var fakeSelectionSet = Mock.Of<ISelectionSet<PhoneNumber>>();

            Action method = () => builder.AddCollectionField(alias, contact => contact.PhoneNumbers, fakeSelectionSet);

            Invoking(method).Should().ThrowExactly<ArgumentException>(because)
                .Where(e => e.ParamName == "alias", "because the alias parameter is the problem")
                .Where(e => e.HelpLink == "https://spec.graphql.org/June2018/#Name", "because this is the link to the GraphQL 'Name' spec");
        }
    }
}