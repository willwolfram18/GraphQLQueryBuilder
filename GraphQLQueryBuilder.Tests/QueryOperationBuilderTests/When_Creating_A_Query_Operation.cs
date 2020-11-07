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

            var expectedOperationName = string.IsNullOrWhiteSpace(operationName) ? null : operationName;
            
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
            
            Invoking(method).Should().ThrowExactly<ArgumentException>(because)
                .Where(e => e.ParamName == "name", "because the name parameter is the problem")
                .Where(e => e.HelpLink == "https://spec.graphql.org/June2018/#Name", "because this is the link to the GraphQL 'Name' spec");
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
                    new FieldSelectionItem(null, nameof(Contact.FirstName), null),
                    new FieldSelectionItem("surname", nameof(Contact.LastName), null),
                    new FieldSelectionItem(null, nameof(Contact.Nicknames), null),
                });
            
            var expectedCustomerSelectionSet = new SelectionSet<Customer>(
                new List<ISelectionSetItem>
                {
                    new FieldSelectionItem(null, nameof(Customer.CustomerContact), expectedContactsSelectionSet)
                });

            var expectedQuerySelections = new List<ISelectionSetItem>
            {
                new FieldSelectionItem(null, nameof(SimpleSchema.Version), null),
                new FieldSelectionItem("foobar", nameof(SimpleSchema.Version), null),
                new FieldSelectionItem(null, nameof(SimpleSchema.PastVersions), null),
                new FieldSelectionItem("versions", nameof(SimpleSchema.PastVersions), null),
                new FieldSelectionItem(null, nameof(SimpleSchema.Administrator), expectedContactsSelectionSet),
                new FieldSelectionItem("admin", nameof(SimpleSchema.Administrator), expectedContactsSelectionSet),
                new FieldSelectionItem(null, nameof(SimpleSchema.Customers), expectedCustomerSelectionSet),
                new FieldSelectionItem("customers", nameof(SimpleSchema.Customers), expectedCustomerSelectionSet),
            };

            queryOperation.Should().NotBeNull();
            queryOperation.Type.Should().Be(OperationTypeForFixture);
            queryOperation.SelectionSet.Should().BeEquivalentTo(expectedQuerySelections, options => options.RespectingRuntimeTypes());
        }
    }
}