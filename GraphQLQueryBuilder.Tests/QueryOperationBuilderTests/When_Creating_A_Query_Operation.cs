using System;
using System.Collections.Generic;
using FluentAssertions;
using GraphQLQueryBuilder.Abstractions.Language;
using GraphQLQueryBuilder.Implementations.Language;
using GraphQLQueryBuilder.Tests.Models;
using NUnit.Framework;
using static FluentAssertions.FluentActions;

namespace GraphQLQueryBuilder.Tests.QueryOperationBuilderTests
{
    public class When_Creating_A_Query_Operation : QueryOperationBuilderTest
    {
        public When_Creating_A_Query_Operation(GraphQLOperationType operationType) : base(operationType)
        {
        }
        
        [Test]
        public void Then_Operation_Type_Matches_Parameter()
        {
            var query = CreateBuilderFor<SimpleSchema>()
                .AddScalarField(schema => schema.Version)
                .Build();

            query.Should().NotBeNull();
            query.Type.Should().Be(OperationTypeForFixture);
            query.Name.Should().BeNull();
        }

        [Test]
        public void Then_Operation_Type_And_Name_Match_Parameters(
            [Values(null, "", "    ", "  \n \t", "MyOperation")] string operationName)
        {
            var query = CreateBuilderFor<SimpleSchema>(operationName)
                .AddScalarField(schema => schema.Version)
                .Build();

            var expectedOperationName = operationName?.Trim();
            
            query.Should().NotBeNull();
            query.Type.Should().Be(OperationTypeForFixture);
            query.Name.Should().Be(expectedOperationName);
        }

        [InvalidOperationNamesTestCaseSource]
        public void
            If_Operation_Name_Is_Not_A_Valid_GraphQL_Name_Then_ArgumentException_Is_Thrown_Stating_Operation_Name_Is_Not_Valid(
                string operationName, string because)
        {
            Action method = () => CreateBuilderFor<SimpleSchema>(operationName);

            Invoking(method).Should().ThrowInvalidGraphQLNameException("name", because);
        }

        [Test]
        public void Then_At_Least_One_Field_Selection_Must_Be_Made()
        {
            var builder = CreateBuilderFor<SimpleSchema>();
            
            Invoking(builder.Build).Should()
                .ThrowExactly<InvalidOperationException>("because a selection set must include 1 or more fields")
                .WithMessage("A selection set must include one or more fields.")
                .Where(e => e.HelpLink == "https://spec.graphql.org/June2018/#SelectionSet");
        }
        
        [Test]
        public void Then_Added_Fields_Are_Included_In_Selections()
        {
            var contactSelectionSet = SelectionSetBuilder.For<Contact>()
                .AddScalarField(contact => contact.FirstName)
                .AddScalarField("surname", contact => contact.LastName)
                .AddScalarCollectionField(contact => contact.Nicknames)
                .Build();

            var customerSelectionSet = SelectionSetBuilder.For<Customer>()
                .AddObjectField(customer => customer.CustomerContact, contactSelectionSet)
                .Build();

            var queryOperation = QueryOperationBuilder.ForSchema<SimpleSchema>(OperationTypeForFixture)
                .AddScalarField(schema => schema.Version)
                .AddScalarField("foobar", schema => schema.Version)
                .AddScalarCollectionField(schema => schema.PastVersions)
                .AddScalarCollectionField("versions", schema => schema.PastVersions)
                .AddObjectField(schema => schema.Administrator, contactSelectionSet)
                .AddObjectField("admin", schema => schema.Administrator, contactSelectionSet)
                .AddObjectCollectionField(schema => schema.Customers, customerSelectionSet)
                .AddObjectCollectionField("customers", schema => schema.Customers, customerSelectionSet)
                .Build();
            
            var expectedContactsSelectionSet = new SelectionSet<Contact>(
                new List<ISelectionSetItem>
                {
                    new ObjectFieldSelectionItem(null, nameof(Contact.FirstName), null),
                    new ObjectFieldSelectionItem("surname", nameof(Contact.LastName), null),
                    new ObjectFieldSelectionItem(null, nameof(Contact.Nicknames), null),
                });
            
            var expectedCustomerSelectionSet = new SelectionSet<Customer>(
                new List<ISelectionSetItem>
                {
                    new ObjectFieldSelectionItem(null, nameof(Customer.CustomerContact), expectedContactsSelectionSet)
                });

            var expectedQuerySelectionSet = new SelectionSet<SimpleSchema>(
                new List<ISelectionSetItem>
                {
                    new ObjectFieldSelectionItem(null, nameof(SimpleSchema.Version), null),
                    new ObjectFieldSelectionItem("foobar", nameof(SimpleSchema.Version), null),
                    new ObjectFieldSelectionItem(null, nameof(SimpleSchema.PastVersions), null),
                    new ObjectFieldSelectionItem("versions", nameof(SimpleSchema.PastVersions), null),
                    new ObjectFieldSelectionItem(null, nameof(SimpleSchema.Administrator), expectedContactsSelectionSet),
                    new ObjectFieldSelectionItem("admin", nameof(SimpleSchema.Administrator), expectedContactsSelectionSet),
                    new ObjectFieldSelectionItem(null, nameof(SimpleSchema.Customers), expectedCustomerSelectionSet),
                    new ObjectFieldSelectionItem("customers", nameof(SimpleSchema.Customers), expectedCustomerSelectionSet),
                });

            var expectedQueryOperation =
                new GraphQLQueryOperation<SimpleSchema>(OperationTypeForFixture, null, expectedQuerySelectionSet);
            
            queryOperation.Should().BeEquivalentTo(expectedQueryOperation, options => options.RespectingRuntimeTypes());
        }
    }
}