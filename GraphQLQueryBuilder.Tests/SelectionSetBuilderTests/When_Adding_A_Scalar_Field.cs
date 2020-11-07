using FluentAssertions;
using GraphQLQueryBuilder.Tests.Models;
using NUnit.Framework;
using System;
using System.Linq.Expressions;
using FluentAssertions.Execution;
using static FluentAssertions.FluentActions;

namespace GraphQLQueryBuilder.Tests.SelectionSetBuilderTests
{
    // TODO: What about primitive aliases like Int32 and Boolean?
    // TODO: test with enums
    public class When_Adding_A_Scalar_Field
    {
        [Test]
        public void If_Property_Expression_Is_Null_Then_ArgumentNullException_Is_Thrown()
        {
            var builder = SelectionSetBuilder.For<Customer>();

            Action addingFieldWithoutAlias = () => builder.AddScalarField<Guid>(null);
            Action addingFieldWithAlias = () => builder.AddScalarField<Guid>("foo", null);

            using (new AssertionScope())
            {
                foreach (var addingField in new[] { addingFieldWithAlias, addingFieldWithoutAlias })
                {
                    Invoking(addingField).Should().ThrowExactly<ArgumentNullException>();
                }
            }
        }

        [Test]
        public void If_Property_Is_A_GraphQL_Object_Type_Then_InvalidOperationException_Is_Thrown_Stating_The_Overload_Should_Be_Used()
        {
            var builder = SelectionSetBuilder.For<Customer>();

            Action addingFieldWithoutAlias = () => builder.AddScalarField(customer => customer.CustomerContact);
            Action addingFieldWithAlias = () => builder.AddScalarField("foo", customer => customer.CustomerContact);

            using (new AssertionScope())
            {
                foreach (var addingField in new [] { addingFieldWithAlias, addingFieldWithoutAlias })
                {
                    Invoking(addingField).Should().ThrowExactly<InvalidOperationException>("because there is an overload for class properties")
                        .WithMessage($"The type '{typeof(Contact).FullName}' is not a GraphQL scalar type.")
                        .Where(e => e.HelpLink == "http://spec.graphql.org/June2018/#sec-Scalars");                    
                }
            }
        }

        [Test]
        public void If_Expression_Is_Not_A_Member_Expression_Then_An_ArgumentException_Is_Thrown()
        {
            var builder = SelectionSetBuilder.For<Customer>();

            Action addingFieldWithoutAlias = () => builder.AddScalarField(_ => "hello");
            Action addingFieldWithAlias = () => builder.AddScalarField("foo", _ => "hello");

            using (new AssertionScope())
            {
                foreach (var addingField in new [] { addingFieldWithAlias, addingFieldWithoutAlias })
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

            Action addingFieldWithoutAlias = () => builder.AddScalarField(customer => customer.CustomerContact.FirstName);
            Action addingFieldWithAlias = () => builder.AddScalarField("foo", customer => customer.CustomerContact.FirstName);

            using (new AssertionScope())
            {
                foreach (var addingField in new [] { addingFieldWithAlias, addingFieldWithoutAlias })
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
            var builder = SelectionSetBuilder.For<Customer>();

            Action method = () => builder.AddScalarField(alias, customer => customer.Id);

            Invoking(method).Should().ThrowExactly<ArgumentException>(because)
                .Where(e => e.ParamName == "alias", "because the alias parameter is the problem")
                .Where(e => e.HelpLink == "https://spec.graphql.org/June2018/#Name", "because this is the link to the GraphQL 'Name' spec");
        }
    }
}