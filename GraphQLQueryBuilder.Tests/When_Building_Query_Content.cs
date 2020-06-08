using FluentAssertions;
using GraphQLQueryBuilder.Tests.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Linq.Expressions;
using static FluentAssertions.FluentActions;

namespace GraphQLQueryBuilder.Tests
{
    public class When_Building_Query_Content : TestClass
    {
        [BadGraphQLNamesTestCaseSource]
        public void Then_Field_Names_Must_Adhere_To_GraphQL_Spec(string fieldName, string becauseReason)
        {
            var queryBuilder = QueryContentBuilder.Of<Address>();
            var fakeSelectionSet = Mock.Of<ISelectionSet>();

            ShouldThrowArgumentException(() => queryBuilder.AddField(fieldName), becauseReason);
            ShouldThrowArgumentException(() => queryBuilder.AddField("aliasA", fieldName), becauseReason);
            ShouldThrowArgumentException(() => queryBuilder.AddField(fieldName, fakeSelectionSet), becauseReason);
            ShouldThrowArgumentException(() => queryBuilder.AddField("aliasA", fieldName, fakeSelectionSet), becauseReason);

            QueryContentShouldMatchSnapshotForTest(queryBuilder);
        }

        [Test]
        public void Then_Field_Name_Must_Be_A_Property_On_The_Class()
        {
            const string fieldName = "I_dont_exist";
            const string becauseReason = "because field name '" + fieldName + "' isn't a property on the class";

            var queryBuilder = QueryContentBuilder.Of<Address>();
            var fakeSelectionSet = Mock.Of<ISelectionSet>();

            ShouldThrowArgumentException(() => queryBuilder.AddField(fieldName), becauseReason);
            ShouldThrowArgumentException(() => queryBuilder.AddField("aliasA", fieldName), becauseReason);
            ShouldThrowArgumentException(() => queryBuilder.AddField(fieldName, fakeSelectionSet), becauseReason);
            ShouldThrowArgumentException(() => queryBuilder.AddField("aliasA", fieldName, fakeSelectionSet), becauseReason);

            QueryContentShouldMatchSnapshotForTest(queryBuilder);
        }

        [Test]
        public void Then_Property_Expression_Cannot_Be_Null()
        {
            var queryBuilder = QueryContentBuilder.Of<Customer>();

            ShouldThrowArgumentNullException(() => queryBuilder.AddField<Contact>(propertyExpression: null));
            ShouldThrowArgumentNullException(() => queryBuilder.AddField<Contact>("aliasA", propertyExpression: null));
            ShouldThrowArgumentNullException(() => queryBuilder.AddField<Contact>("aliasA", propertyExpression: null, Mock.Of<ISelectionSet<Contact>>()));

            QueryContentShouldMatchSnapshotForTest(queryBuilder);
        }

        [Test]
        public void Then_Property_Expression_Must_Be_A_Field_On_The_Class()
        {
            Expression<Func<Customer, Address>> invalidPropertyExpression = customer => customer.CustomerContact.Address;
            const string becauseReason = "because property expression isn't a property of the class";

            var queryBuilder = QueryContentBuilder.Of<Customer>();
            var fakeSelectionSet = Mock.Of<ISelectionSet<Address>>();

            Invoking(() => queryBuilder.AddField(invalidPropertyExpression)).Should().Throw<InvalidOperationException>(becauseReason);
            Invoking(() => queryBuilder.AddField("aliasA", invalidPropertyExpression)).Should().Throw<InvalidOperationException>(becauseReason);
            Invoking(() => queryBuilder.AddField(invalidPropertyExpression, fakeSelectionSet)).Should().Throw<InvalidOperationException>(becauseReason);
            Invoking(() => queryBuilder.AddField("aliasA", invalidPropertyExpression, fakeSelectionSet)).Should().Throw<InvalidOperationException>(becauseReason);

            QueryContentShouldMatchSnapshotForTest(queryBuilder);
        }

        [BadGraphQLNamesTestCaseSource]
        public void Then_Alias_Name_Must_Adhere_To_GraphQL_Spec(string aliasName, string becauseReason)
        {
            const string validFieldName = nameof(Customer.CustomerContact);
            Expression<Func<Customer, Contact>> validFieldExpression = customer => customer.CustomerContact;
            var queryBuilder = QueryContentBuilder.Of<Customer>();
            var fakeSelectionSet = Mock.Of<ISelectionSet<Contact>>();

            ShouldThrowArgumentException(() => queryBuilder.AddField(aliasName, validFieldName), becauseReason);
            ShouldThrowArgumentException(() => queryBuilder.AddField(aliasName, validFieldName, fakeSelectionSet), becauseReason);
            ShouldThrowArgumentException(() => queryBuilder.AddField(aliasName, validFieldExpression), becauseReason);
            ShouldThrowArgumentException(() => queryBuilder.AddField(aliasName, validFieldExpression, fakeSelectionSet), becauseReason);

            QueryContentShouldMatchSnapshotForTest(queryBuilder);
        }

        [Test]
        public void Then_Selection_Set_Cannot_Be_Null()
        {
            const string validFieldName = nameof(Customer.CustomerContact);
            Expression<Func<Customer, Contact>> validFieldExpression = customer => customer.CustomerContact;
            var queryBuilder = QueryContentBuilder.Of<Customer>();

            ShouldThrowArgumentNullException(() => queryBuilder.AddField(validFieldExpression, null));
            ShouldThrowArgumentNullException(() => queryBuilder.AddField("aliasA", validFieldExpression, null));
            ShouldThrowArgumentNullException(() => queryBuilder.AddField(validFieldName, selectionSet: null));
            ShouldThrowArgumentNullException(() => queryBuilder.AddField("aliasA", validFieldName, null));

            QueryContentShouldMatchSnapshotForTest(queryBuilder);
        }

        [Test]
        public void Then_Property_Expressions_Are_Included_In_Content()
        {
            var fakeContactSelectionSet = Mock.Of<ISelectionSet<Contact>>(contact =>
                contact.Build() == "{ id, name }"
            );
            var queryBuilder = QueryContentBuilder.Of<Customer>()
                .AddField(customer => customer.Id)
                .AddField("aliasA", customer => customer.AccountNumber)
                .AddField(customer => customer.CustomerContact, fakeContactSelectionSet)
                .AddField("aliasB", customer => customer.CustomerContact, fakeContactSelectionSet);

            QueryContentShouldMatchSnapshotForTest(queryBuilder);
        }

        [Test]
        public void Then_Added_Field_Names_Are_Included_In_Content()
        {
            var fakeContactSelectionSet = Mock.Of<ISelectionSet<Contact>>(contact =>
                contact.Build() == "{ id, name }"
            );

            var queryBuilder = QueryContentBuilder.Of<Customer>()
                .AddField(nameof(Customer.Id))
                .AddField("aliasA", nameof(Customer.AccountNumber))
                .AddField(nameof(Customer.CustomerContact), fakeContactSelectionSet)
                .AddField("aliasB", nameof(Customer.CustomerContact), fakeContactSelectionSet);

            QueryContentShouldMatchSnapshotForTest(queryBuilder);
        }

        private static void ShouldThrowArgumentNullException<T>(Func<T> action)
        {
            action.Should().Throw<ArgumentNullException>();
        }

        private static void ShouldThrowArgumentException<T>(Func<T> action, string becauseReason)
        {
            action.Should().Throw<ArgumentException>(becauseReason);
        }
    }
}