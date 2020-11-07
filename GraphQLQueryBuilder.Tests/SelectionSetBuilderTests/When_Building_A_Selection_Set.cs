using System;
using System.Collections.Generic;
using FluentAssertions;
using GraphQLQueryBuilder.Abstractions.Language;
using GraphQLQueryBuilder.Implementations.Language;
using GraphQLQueryBuilder.Tests.Models;
using NUnit.Framework;
using static FluentAssertions.FluentActions;

namespace GraphQLQueryBuilder.Tests.SelectionSetBuilderTests
{
    public class When_Building_A_Selection_Set
    {
        [Test]
        public void Then_Added_Properties_Are_In_The_Selection_Set()
        {
            var selectionSet = SelectionSetBuilder.For<Customer>()
                .AddField(customer => customer.Id)
                .AddField("acctNum", customer => customer.AccountNumber)
                .Build();

            var expectedSelections = new List<IFieldSelectionItem>
            {
                new FieldSelectionItem(null, nameof(Customer.Id), null),
                new FieldSelectionItem("acctNum", nameof(Customer.AccountNumber), null)
            };

            selectionSet.Should().NotBeNull();
            selectionSet.Selections.Should().BeEquivalentTo(expectedSelections, options => options.RespectingRuntimeTypes());
        }

        [Test]
        public void If_Added_Properties_Are_Classes_Then_They_Are_In_The_Selection_Set()
        {
            var contactSelectionSet = SelectionSetBuilder.For<Contact>()
                .AddField(contact => contact.FirstName)
                .AddField(contact => contact.LastName)
                .Build();

            var customerSelectionSet = SelectionSetBuilder.For<Customer>()
                .AddField(customer => customer.Id)
                .AddField(customer => customer.CustomerContact, contactSelectionSet)
                .AddField("foobar", customer => customer.CustomerContact, contactSelectionSet)
                .Build();
            
            var expectedContactSelectionSet = new SelectionSet<Contact>(
                new ISelectionSetItem[]
                {
                    new FieldSelectionItem(null, nameof(Contact.FirstName), null),
                    new FieldSelectionItem(null, nameof(Contact.LastName), null),
                });
            var expectedCustomerSelections = new List<ISelectionSetItem>
            {
                new FieldSelectionItem(null, nameof(Customer.Id), null),
                new FieldSelectionItem(null, nameof(Customer.CustomerContact), expectedContactSelectionSet),
                new FieldSelectionItem("foobar", nameof(Customer.CustomerContact), expectedContactSelectionSet),
            };

            customerSelectionSet.Should().NotBeNull();
            customerSelectionSet.Selections.Should().BeEquivalentTo(expectedCustomerSelections, options => options.RespectingRuntimeTypes());
        }

        [Test]
        public void If_Added_Properties_Are_Collection_Then_They_Are_In_The_Selection_Set()
        {
            var phoneNumberSelectionSet = SelectionSetBuilder.For<PhoneNumber>()
                .AddField(phone => phone.Number)
                .AddField("ext", phone => phone.Extension)
                .Build();

            var contactSelectionSet = SelectionSetBuilder.For<Contact>()
                .AddField(contact => contact.FirstName)
                .AddField("surname", contact => contact.LastName)
                .AddCollectionField(contact => contact.PhoneNumbers, phoneNumberSelectionSet)
                .AddCollectionField("foobar", contact => contact.PhoneNumbers, phoneNumberSelectionSet)
                .AddCollectionField("names", contact => contact.Nicknames)
                .Build();

            var customerSelectionSet = SelectionSetBuilder.For<Customer>()
                .AddField(customer => customer.Id)
                .AddCollectionField(customer => customer.FavoriteNumbers)
                .AddCollectionField("baz", customer => customer.FavoriteNumbers)
                .AddField(customer => customer.CustomerContact)
                .Build();
            
            var expectedPhoneNumberSelectionSet = new SelectionSet<PhoneNumber>(
                new List<ISelectionSetItem>
                {
                    new FieldSelectionItem(null, nameof(PhoneNumber.Number), null),
                    new FieldSelectionItem("ext", nameof(PhoneNumber.Extension), null)
                });

            var expectedContactSelection = new SelectionSet<Contact>(
                new List<ISelectionSetItem>
                {
                    new FieldSelectionItem(null, nameof(Contact.FirstName), null),
                    new FieldSelectionItem("surname", nameof(Contact.LastName), null),
                    new FieldSelectionItem(null, nameof(Contact.PhoneNumbers), expectedPhoneNumberSelectionSet),
                    new FieldSelectionItem("foobar", nameof(Contact.PhoneNumbers), expectedPhoneNumberSelectionSet),
                    new FieldSelectionItem("names", nameof(Contact.Nicknames), null)
                });

            var expectedCustomerSelections = new List<ISelectionSetItem>
            {
                new FieldSelectionItem(null, nameof(Customer.Id), null),
                new FieldSelectionItem(null, nameof(Customer.FavoriteNumbers), null),
                new FieldSelectionItem("baz", nameof(Customer.FavoriteNumbers), null),
                new FieldSelectionItem(null, nameof(Customer.CustomerContact), expectedContactSelection)
            };

            customerSelectionSet.Should().NotBeNull();
            customerSelectionSet.Selections.Should().BeEquivalentTo(expectedCustomerSelections, options => options.RespectingRuntimeTypes());
        }
        
        [Test]
        public void If_No_Fields_Are_Selected_Before_Building_Then_An_InvalidOperationException_Is_Thrown()
        {
            var builder = SelectionSetBuilder.For<Customer>();

            Invoking(builder.Build).Should()
                .ThrowExactly<InvalidOperationException>("because a selection set must include 1 or more fields")
                .WithMessage("A selection set must include one or more fields.")
                .Where(e => e.HelpLink == "https://spec.graphql.org/June2018/#SelectionSet");
        }
    }
}