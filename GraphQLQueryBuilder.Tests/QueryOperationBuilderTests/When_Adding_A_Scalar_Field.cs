using System;
using System.Linq.Expressions;
using FluentAssertions;
using FluentAssertions.Execution;
using GraphQLQueryBuilder.Abstractions.Language;
using GraphQLQueryBuilder.Tests.Models;
using NUnit.Framework;
using static FluentAssertions.FluentActions;

namespace GraphQLQueryBuilder.Tests.QueryOperationBuilderTests
{
    
    public class When_Adding_A_Scalar_Field : QueryOperationBuilderTest
    {
        public When_Adding_A_Scalar_Field(GraphQLOperationType operationType) : base(operationType)
        {
        }
        
        [Test]
        public void If_Property_Expression_Is_Null_Then_ArgumentNullException_Is_Thrown()
        {
            var builder = CreateBuilderFor<SimpleSchema>();

            Action addingFieldWithoutAlias = () => builder.AddScalarField<string>(null);
            Action addingFieldWithAlias = () => builder.AddScalarField<string>("foo", null);

            using (new AssertionScope())
            {
                foreach (var addingField in new[] { addingFieldWithAlias, addingFieldWithoutAlias })
                {
                    Invoking(addingField).Should().ThrowExactly<ArgumentNullException>();
                }
            }
        }

        [Test]
        public void If_Property_Type_Is_Not_A_GraphQL_Scalar_Then_InvalidOperationException_Is_Thrown()
        {
            var builder = CreateBuilderFor<SimpleSchema>();

            Action addingFieldWithoutAlias = () => builder.AddScalarField(schema => schema.Administrator);
            Action addingFieldWithAlias = () => builder.AddScalarField("foo", schema => schema.Administrator);

            using (new AssertionScope())
            {
                foreach (var addingField in new [] { addingFieldWithAlias, addingFieldWithoutAlias })
                {
                    Invoking(addingField).Should().ThrowExactly<InvalidOperationException>("because there is an overload for class properties")
                        .WithMessage($"The type '{typeof(Contact).FullName}' is not a GraphQL scalar type.");                    
                }
            }
        }

        [Test]
        public void If_Expression_Is_Not_A_Member_Expression_Then_An_ArgumentException_Is_Thrown()
        {
            var builder = CreateBuilderFor<SimpleSchema>();

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
            var builder = CreateBuilderFor<SimpleSchema>();

            Action addingFieldWithoutAlias = () => builder.AddScalarField(schema => schema.Administrator.FirstName);
            Action addingFieldWithAlias = () => builder.AddScalarField("foo", schema => schema.Administrator.FirstName);

            using (new AssertionScope())
            {
                foreach (var addingField in new [] { addingFieldWithAlias, addingFieldWithoutAlias })
                {
                    Invoking(addingField).Should().ThrowExactly<InvalidOperationException>()
                        .WithMessage($"Property '*' is not a member of type '{typeof(SimpleSchema).FullName}'.");
                }
            }
        }

        [InvalidGraphQLNamesTestCaseSource]
        public void If_Property_Alias_Is_Not_A_Valid_GraphQL_Name_Then_ArgumentException_Is_Thrown_Stating_Alias_Is_Not_Valid(
            string alias, string because)
        {
            var builder = CreateBuilderFor<SimpleSchema>();

            Action method = () => builder.AddScalarField(alias, schema => schema.Version);

            Invoking(method).Should().ThrowInvalidGraphQLNameException("alias", because);
        }
    }
}