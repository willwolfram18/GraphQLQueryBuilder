using FluentAssertions;
using FluentAssertions.Execution;
using GraphQLQueryBuilder.Abstractions.Language;
using GraphQLQueryBuilder.Tests.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using static FluentAssertions.FluentActions;

namespace GraphQLQueryBuilder.Tests.SelectionSetBuilderTests
{
    public class When_Adding_A_Field_That_Is_A_Class
    {
        [Test]
        public void If_Property_Expression_Is_Null_Then_ArgumentNullException_Is_Thrown()
        {
            var builder = SelectionSetBuilder.Of<Customer>();
            var fakeSelectionSet = Mock.Of<ISelectionSet<Contact>>();

            Action addingFieldWithoutAlias = () => builder.AddField(null, fakeSelectionSet);
            Action addingFieldWithAlias = () => builder.AddField("foo", null, fakeSelectionSet);

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
            var builder = SelectionSetBuilder.Of<Customer>();

            Action addingFieldWithoutAlias = () => builder.AddField(customer => customer.CustomerContact, null);
            Action addingFieldWithAlias = () => builder.AddField("foo", customer => customer.CustomerContact, null);

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
            var builder = SelectionSetBuilder.Of<Customer>();
            var fakeSelectionSet = Mock.Of<ISelectionSet<string>>();

            Action addingFieldWithoutAlias = () => builder.AddField(_ => "hello", fakeSelectionSet);
            Action addingFieldWithAlias = () => builder.AddField("foo", _ => "hello", fakeSelectionSet);

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
            var builder = SelectionSetBuilder.Of<Customer>();
            var fakeSelectionSet = Mock.Of<ISelectionSet<Address>>();

            Action addingFieldWithoutAlias = () =>
                builder.AddField(customer => customer.CustomerContact.Address, fakeSelectionSet);
            Action addingFieldWithAlias = () =>
                builder.AddField("foo", customer => customer.CustomerContact.Address, fakeSelectionSet);

            using (new AssertionScope())
            {
                foreach (var addingField in new [] { addingFieldWithAlias, addingFieldWithoutAlias})
                {
                    Invoking(addingField).Should().ThrowExactly<InvalidOperationException>()
                        .WithMessage($"Property '*' is not a member of type '{typeof(Customer).FullName}'.");
                }
            }
        }

        [Test]
        public void If_Expression_Is_A_String_Property_Then_An_InvalidOperationException_Is_Thrown_Stating_To_Use_The_Other_Method_Overload()
        {
            var builder = SelectionSetBuilder.Of<Customer>();
            var fakeSelectionSet = Mock.Of<ISelectionSet<string>>();

            Action addingFieldWithoutAlias = () =>
                builder.AddField(customer => customer.AccountNumber, fakeSelectionSet);
            Action addingFieldWithAlias = () =>
                builder.AddField("foo", customer => customer.AccountNumber, fakeSelectionSet);

            using (new AssertionScope())
            {
                foreach (var addingField in new [] { addingFieldWithAlias, addingFieldWithoutAlias })
                {
                    Invoking(addingField).Should().ThrowExactly<InvalidOperationException>()
                        .WithMessage(
                            $"When selecting a property that is of type string, please use the {nameof(builder.AddField)} method that does not take an {nameof(ISelectionSet)}.");
                }
            }
        }

        [Test]
        public void If_Expression_Is_An_Enumerable_Property_Then_An_InvalidOperationException_Is_Thrown_Stating_To_Use_AddCollectionField()
        {
            var builder = SelectionSetBuilder.Of<Contact>();
            var fakeSelectionSet = Mock.Of<ISelectionSet<IEnumerable<PhoneNumber>>>();

            Action addingFieldWithoutAlias = () =>
                builder.AddField(contact => contact.PhoneNumbers, fakeSelectionSet);
            Action addingFieldWithAlias = () =>
                builder.AddField("foo", contact => contact.PhoneNumbers, fakeSelectionSet);

            using (new AssertionScope())
            {
                foreach (var addingField in new [] { addingFieldWithAlias, addingFieldWithoutAlias})
                {
                    Invoking(addingField).Should().ThrowExactly<InvalidOperationException>()
                        .WithMessage(
                            $"When selecting a property that is an IEnumerable, please use the {nameof(builder.AddCollectionField)} method.");
                }
            }
        }

        [InvalidAliasNamesTestCaseSource]
        public void If_Property_Alias_Is_Not_A_Valid_GraphQL_Name_Then_ArgumentException_Is_Thrown_Stating_Alias_Is_Not_Valid(
            string alias, string because)
        {
            var builder = SelectionSetBuilder.Of<Customer>();
            var fakeSelectionSet = Mock.Of<ISelectionSet<Contact>>();

            Action method = () => builder.AddField(alias, customer => customer.CustomerContact, fakeSelectionSet);

            Invoking(method).Should().ThrowExactly<ArgumentException>(because)
                .Where(e => e.ParamName == "alias", "because the alias parameter is the problem")
                .Where(e => e.HelpLink == "https://spec.graphql.org/June2018/#Name", "because this is the link to the GraphQL 'Name' spec");
        }
    }
}
