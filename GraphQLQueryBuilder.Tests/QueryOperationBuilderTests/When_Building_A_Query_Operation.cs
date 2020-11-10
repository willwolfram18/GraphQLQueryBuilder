using FluentAssertions;
using GraphQLQueryBuilder.Abstractions.Language;
using GraphQLQueryBuilder.Implementations.Language;
using GraphQLQueryBuilder.Tests.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using static FluentAssertions.FluentActions;

namespace GraphQLQueryBuilder.Tests.QueryOperationBuilderTests
{
    public class When_Building_A_Query_Operation : QueryOperationBuilderTest
    {
        public When_Building_A_Query_Operation(GraphQLOperationType operationType) : base(operationType)
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

        [Test]
        public void Then_Arguments_Are_Included_In_Field_Selections()
        {
            var customerSelectionSet = SelectionSetBuilder.For<Customer>()
                .AddScalarField(customer => customer.Id)
                .Build();

            var versionArgs = new ArgumentCollection
            {
                ArgumentBuilder.Build("foo", "hello world"),
                ArgumentBuilder.Build("bar", 10),
                ArgumentBuilder.Build("baz")
            };
            var expectedVersionArgs = new IArgument[versionArgs.Count];

            // copy the original arguments so our expectations aren't contaminated by the method under test
            versionArgs.CopyTo(expectedVersionArgs, 0);

            var customersArgs = new ArgumentCollection
            {
                ArgumentBuilder.Build("isActive", true),
                ArgumentBuilder.Build("age", 50.2)
            };
            var expectedCustomersArgs = new IArgument[customersArgs.Count];

            // copy the original arguments so our expectations aren't contaminated by the method under test
            customersArgs.CopyTo(expectedCustomersArgs, 0);

            var queryOperation = QueryOperationBuilder.ForSchema<SimpleSchema>(OperationTypeForFixture)
                .AddScalarCollectionField(schema => schema.PastVersions, versionArgs)
                .AddScalarCollectionField("versions", schema => schema.PastVersions, versionArgs)
                .AddObjectCollectionField(schema => schema.Customers, customersArgs, customerSelectionSet)
                .AddObjectCollectionField("foobar", schema => schema.Customers, customersArgs, customerSelectionSet)
                .Build();

            var expectedCustomerSelectionSet = new SelectionSet<Customer>(new List<ISelectionSetItem>
            {
                new ScalarFieldSelectionItem(null, nameof(Customer.Id))
            });

            var expectedSchemaSelections = new List<ISelectionSetItem>
            {
                new ScalarFieldSelectionItem(null, nameof(SimpleSchema.PastVersions), expectedVersionArgs),
                new ScalarFieldSelectionItem("versions", nameof(SimpleSchema.PastVersions), expectedVersionArgs),
                new ObjectFieldSelectionItem(null, nameof(SimpleSchema.Customers), expectedCustomersArgs,
                    expectedCustomerSelectionSet),
                new ObjectFieldSelectionItem("foobar", nameof(SimpleSchema.Customers), expectedCustomersArgs,
                    expectedCustomerSelectionSet),
            };

            queryOperation.Should().NotBeNull();
            queryOperation.SelectionSet.Selections.Should().BeEquivalentTo(expectedSchemaSelections,
                options => options.RespectingRuntimeTypes());
        }

        public static IEnumerable<IArgumentCollection> NullOrEmptyArguments => new[]
        {
            null,
            new ArgumentCollection(),
        };

        [TestCaseSource(nameof(NullOrEmptyArguments))]
        public void If_Arguments_For_Scalar_Fields_Are_Null_Or_Empty_Then_Arguments_For_Field_Selection_Are_Empty(
            IArgumentCollection arguments)
        {
            var query = QueryOperationBuilder.ForSchema<SimpleSchema>(OperationTypeForFixture)
                .AddScalarField(schema => schema.Version, arguments)
                .AddScalarField("foobar", schema => schema.Version, arguments)
                .Build();

            var expectedSelections = new List<ISelectionSetItem>
            {
                new ScalarFieldSelectionItem(null, nameof(SimpleSchema.Version), Enumerable.Empty<IArgument>()),
                new ScalarFieldSelectionItem("foobar", nameof(SimpleSchema.Version), Enumerable.Empty<IArgument>()),
            };

            query.Should().NotBeNull();
            query.SelectionSet.Selections.Should()
                .BeEquivalentTo(expectedSelections, options => options.RespectingRuntimeTypes());
        }

        [TestCaseSource(nameof(NullOrEmptyArguments))]
        public void If_Arguments_For_Object_Fields_Are_Null_Or_Empty_Then_Arguments_For_Field_Selection_Are_Empty(
            IArgumentCollection arguments)
        {
            var contactSelectionSet = SelectionSetBuilder.For<Contact>()
                .AddScalarField(contact => contact.FirstName)
                .Build();

            var query = QueryOperationBuilder.ForSchema<SimpleSchema>(OperationTypeForFixture)
                .AddObjectField(schema => schema.Administrator, arguments, contactSelectionSet)
                .AddObjectField("foobar", schema => schema.Administrator, arguments, contactSelectionSet)
                .Build();

            var expectedContactSelectionSet = new SelectionSet<Contact>(new List<ISelectionSetItem>
            {
                new ScalarFieldSelectionItem(null, nameof(Contact.FirstName))
            });
            var expectedSelections = new List<ISelectionSetItem>
            {
                new ObjectFieldSelectionItem(null, nameof(SimpleSchema.Administrator), Enumerable.Empty<IArgument>(), expectedContactSelectionSet),
                new ObjectFieldSelectionItem("foobar", nameof(SimpleSchema.Administrator), Enumerable.Empty<IArgument>(), expectedContactSelectionSet),
            };

            query.Should().NotBeNull();
            query.SelectionSet.Selections.Should()
                .BeEquivalentTo(expectedSelections, options => options.RespectingRuntimeTypes());
        }

        [TestCaseSource(nameof(NullOrEmptyArguments))]
        public void If_Arguments_For_Scalar_Collection_Fields_Are_Null_Or_Empty_Then_Arguments_For_Field_Selection_Are_Empty(
            IArgumentCollection arguments)
        {
            var query = QueryOperationBuilder.ForSchema<SimpleSchema>(OperationTypeForFixture)
                .AddScalarCollectionField(schema => schema.PastVersions, arguments)
                .AddScalarCollectionField("foobar", schema => schema.PastVersions, arguments)
                .Build();

            var expectedSelections = new List<ISelectionSetItem>
            {
                new ScalarFieldSelectionItem(null, nameof(SimpleSchema.PastVersions), Enumerable.Empty<IArgument>()),
                new ScalarFieldSelectionItem("foobar", nameof(SimpleSchema.PastVersions), Enumerable.Empty<IArgument>()),
            };

            query.Should().NotBeNull();
            query.SelectionSet.Selections.Should()
                .BeEquivalentTo(expectedSelections, options => options.RespectingRuntimeTypes());
        }

        [TestCaseSource(nameof(NullOrEmptyArguments))]
        public void If_Arguments_For_Object_Collection_Fields_Are_Null_Or_Empty_Then_Arguments_For_Field_Selection_Are_Empty(
            IArgumentCollection arguments)
        {
            var customerSelectionSet = SelectionSetBuilder.For<Customer>()
                .AddScalarField(customer => customer.Id)
                .Build();

            var query = QueryOperationBuilder.ForSchema<SimpleSchema>(OperationTypeForFixture)
                .AddObjectCollectionField(schema => schema.Customers, arguments, customerSelectionSet)
                .AddObjectCollectionField("foobar", schema => schema.Customers, arguments, customerSelectionSet)
                .Build();

            var expectedCustomerSelectionSet = new SelectionSet<Customer>(new List<ISelectionSetItem>
            {
                new ScalarFieldSelectionItem(null, nameof(Customer.Id))
            });
            var expectedSelections = new List<ISelectionSetItem>
            {
                new ObjectFieldSelectionItem(null, nameof(SimpleSchema.Customers), Enumerable.Empty<IArgument>(), expectedCustomerSelectionSet),
                new ObjectFieldSelectionItem("foobar", nameof(SimpleSchema.Customers), Enumerable.Empty<IArgument>(), expectedCustomerSelectionSet),
            };

            query.Should().NotBeNull();
            query.SelectionSet.Selections.Should()
                .BeEquivalentTo(expectedSelections, options => options.RespectingRuntimeTypes());
        }
    }
}