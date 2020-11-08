using System;
using System.Linq.Expressions;
using FluentAssertions;
using FluentAssertions.Execution;
using GraphQLQueryBuilder.Tests.Models;
using NUnit.Framework;
using static FluentAssertions.FluentActions;

namespace GraphQLQueryBuilder.Tests.SelectionSetBuilderTests
{
    public class When_Adding_A_Scalar_Collection
    {
        [Test]
        public void If_Property_Expression_Is_Null_Then_ArgumentNullException_Is_Thrown()
        {
            var builder = SelectionSetBuilder.For<Contact>();

            Action addingFieldWithoutAlias = () => builder.AddScalarCollectionField<Contact, Guid>(null);
            Action addingFieldWithAlias = () => builder.AddScalarCollectionField<Contact, Guid>("foo", null);

            using (new AssertionScope())
            {
                foreach (var addingField in new [] { addingFieldWithAlias, addingFieldWithoutAlias})
                {
                    Invoking(addingField).Should().ThrowExactly<ArgumentNullException>()
                        .Where(e => e.ParamName == "expression");
                }
            }
        }
        
        [InvalidGraphQLNamesTestCaseSource]
        public void If_Property_Alias_Is_Not_A_Valid_GraphQL_Name_Then_ArgumentException_Is_Thrown_Stating_Alias_Is_Not_Valid(
            string alias, string because)
        {
            var builder = SelectionSetBuilder.For<Contact>();

            Action method = () => builder.AddScalarCollectionField(alias, contact => contact.Nicknames);

            Invoking(method).Should().ThrowInvalidGraphQLNameException("alias", because);
        }
        
        [Test]
        public void If_Expression_Is_Not_A_Member_Expression_Then_An_ArgumentException_Is_Thrown()
        {
            var builder = SelectionSetBuilder.For<Contact>();

            Action addingFieldWithoutAlias = () => builder.AddScalarCollectionField(_ => new [] { "hello" });
            Action addingFieldWithAlias = () => builder.AddScalarCollectionField("foo", _ => new [] { "hello" });

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

            Action addingFieldWithoutAlias = () =>
                builder.AddScalarCollectionField(customer => customer.CustomerContact.Nicknames);
            Action addingFieldWithAlias = () =>
                builder.AddScalarCollectionField("foo", customer => customer.CustomerContact.Nicknames);

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
        public void If_Property_Is_A_GraphQL_Object_Type_Then_InvalidOperationException_Is_Thrown()
        {
            var builder = SelectionSetBuilder.For<Contact>();

            Action addingFieldWithoutAlias = () => builder.AddScalarCollectionField(contact => contact.PhoneNumbers);
            Action addingFieldWithAlias = () => builder.AddScalarCollectionField("foo", contact => contact.PhoneNumbers);

            using (new AssertionScope())
            {
                foreach (var addingField in new [] { addingFieldWithAlias, addingFieldWithoutAlias })
                {
                    Invoking(addingField).Should().ThrowExactly<InvalidOperationException>("because there is an overload for class properties")
                        .WithMessage($"The type '{typeof(PhoneNumber).FullName}' is not a GraphQL scalar type.")
                        .Where(e => e.HelpLink == "http://spec.graphql.org/June2018/#sec-Scalars");                    
                }
            }
        }
    }
}