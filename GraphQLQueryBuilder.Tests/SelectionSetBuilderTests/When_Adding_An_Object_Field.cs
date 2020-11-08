using FluentAssertions;
using FluentAssertions.Execution;
using GraphQLQueryBuilder.Abstractions.Language;
using GraphQLQueryBuilder.Tests.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using static FluentAssertions.FluentActions;

namespace GraphQLQueryBuilder.Tests.SelectionSetBuilderTests
{
    public class When_Adding_An_Object_Field
    {
        [Test]
        public void If_Property_Expression_Is_Null_Then_ArgumentNullException_Is_Thrown()
        {
            var builder = SelectionSetBuilder.For<Customer>();
            var fakeSelectionSet = Mock.Of<ISelectionSet<Contact>>();

            Action addingFieldWithoutAlias = () => builder.AddObjectField(null, fakeSelectionSet);
            Action addingFieldWithAlias = () => builder.AddObjectField("foo", null, fakeSelectionSet);

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
            var builder = SelectionSetBuilder.For<Customer>();

            Action addingFieldWithoutAlias = () => builder.AddObjectField(customer => customer.CustomerContact, null);
            Action addingFieldWithAlias = () => builder.AddObjectField("foo", customer => customer.CustomerContact, null);

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
            var builder = SelectionSetBuilder.For<Customer>();
            var fakeSelectionSet = Mock.Of<ISelectionSet<string>>();

            Action addingFieldWithoutAlias = () => builder.AddObjectField(_ => "hello", fakeSelectionSet);
            Action addingFieldWithAlias = () => builder.AddObjectField("foo", _ => "hello", fakeSelectionSet);

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
            var fakeSelectionSet = Mock.Of<ISelectionSet<Address>>();

            Action addingFieldWithoutAlias = () =>
                builder.AddObjectField(customer => customer.CustomerContact.Address, fakeSelectionSet);
            Action addingFieldWithAlias = () =>
                builder.AddObjectField("foo", customer => customer.CustomerContact.Address, fakeSelectionSet);

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
            var builder = SelectionSetBuilder.For<Customer>();
            var fakeSelectionSet = Mock.Of<ISelectionSet<string>>();

            Action addingFieldWithoutAlias = () =>
                builder.AddObjectField(customer => customer.AccountNumber, fakeSelectionSet);
            Action addingFieldWithAlias = () =>
                builder.AddObjectField("foo", customer => customer.AccountNumber, fakeSelectionSet);

            using (new AssertionScope())
            {
                foreach (var addingField in new [] { addingFieldWithAlias, addingFieldWithoutAlias })
                {
                    Invoking(addingField).Should().ThrowExactly<InvalidOperationException>()
                        .WithMessage(
                            $"The type '{typeof(string).FullName}' is not a GraphQL object type. Use the {nameof(builder.AddScalarField)} method.")
                        .Where(e => e.HelpLink == "http://spec.graphql.org/June2018/#sec-Objects");
                }
            }
        }

        [Test]
        public void If_Expression_Is_An_Enumerable_Property_Then_An_InvalidOperationException_Is_Thrown_Stating_To_Use_AddCollectionField()
        {
            var builder = SelectionSetBuilder.For<Contact>();
            var fakeSelectionSet = Mock.Of<ISelectionSet<IEnumerable<PhoneNumber>>>();

            Action addingFieldWithoutAlias = () =>
                builder.AddObjectField(contact => contact.PhoneNumbers, fakeSelectionSet);
            Action addingFieldWithAlias = () =>
                builder.AddObjectField("foo", contact => contact.PhoneNumbers, fakeSelectionSet);

            using (new AssertionScope())
            {
                foreach (var addingField in new [] { addingFieldWithAlias, addingFieldWithoutAlias})
                {
                    Invoking(addingField).Should().ThrowExactly<InvalidOperationException>()
                        .WithMessage(
                            $"The type '{typeof(IEnumerable<PhoneNumber>).FullName}' is a GraphQL list type. Use the {nameof(builder.AddScalarCollectionField)} or {nameof(builder.AddObjectCollectionField)} methods.")
                        .Where(e => e.HelpLink == "http://spec.graphql.org/June2018/#sec-Type-System.List");
                }
            }
        }

        [InvalidGraphQLNamesTestCaseSource]
        public void If_Property_Alias_Is_Not_A_Valid_GraphQL_Name_Then_ArgumentException_Is_Thrown_Stating_Alias_Is_Not_Valid(
            string alias, string because)
        {
            var builder = SelectionSetBuilder.For<Customer>();
            var fakeSelectionSet = Mock.Of<ISelectionSet<Contact>>();

            Action method = () => builder.AddObjectField(alias, customer => customer.CustomerContact, fakeSelectionSet);

            Invoking(method).Should().ThrowInvalidGraphQLNameException("alias", because);
        }
    }
}
