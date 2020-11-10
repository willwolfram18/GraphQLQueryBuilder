using FluentAssertions;
using GraphQLQueryBuilder.Abstractions.Language;
using GraphQLQueryBuilder.Implementations.Language;
using GraphQLQueryBuilder.Tests.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using static FluentAssertions.FluentActions;

namespace GraphQLQueryBuilder.Tests.SelectionSetBuilderTests
{
    public class When_Building_A_Selection_Set
    {
        public static IEnumerable<IArgumentCollection> NullOrEmptyArguments => new[]
        {
            null,
            new ArgumentCollection(),
        };

        [TestCaseSource(nameof(NullOrEmptyArguments))]
        public void If_Arguments_For_Scalar_Fields_Are_Null_Or_Empty_Then_Arguments_For_Field_Selection_Are_Empty(
            IArgumentCollection arguments)
        {
            var selectionSet = SelectionSetBuilder.For<Customer>()
                .AddScalarField(customer => customer.Id, arguments)
                .AddScalarField("foobar", customer => customer.Id, arguments)
                .Build();

            var expectedSelections = new ISelectionSetItem[]
            {
                new ScalarFieldSelectionItem(null, nameof(Customer.Id)),
                new ScalarFieldSelectionItem("foobar", nameof(Customer.Id)),
            };

            selectionSet.Should().NotBeNull();
            selectionSet.Selections.Should()
                .BeEquivalentTo(expectedSelections, options => options.RespectingRuntimeTypes());
        }

        [TestCaseSource(nameof(NullOrEmptyArguments))]
        public void If_Arguments_For_Scalar_Collection_Fields_Are_Null_Or_Empty_Then_Arguments_For_Field_Selection_Are_Empty(
            IArgumentCollection arguments)
        {
            var selectionSet = SelectionSetBuilder.For<Customer>()
                .AddScalarCollectionField(customer => customer.FavoriteNumbers, arguments)
                .AddScalarCollectionField("numbers", customer => customer.FavoriteNumbers, arguments)
                .Build();

            var expectedSelections = new ISelectionSetItem[]
            {
                new ScalarFieldSelectionItem(null, nameof(Customer.FavoriteNumbers)),
                new ScalarFieldSelectionItem("numbers", nameof(Customer.FavoriteNumbers)),
            };

            selectionSet.Should().NotBeNull();
            selectionSet.Selections.Should()
                .BeEquivalentTo(expectedSelections, options => options.RespectingRuntimeTypes());
        }

        [TestCaseSource(nameof(NullOrEmptyArguments))]
        public void If_Arguments_For_Object_Fields_Are_Null_Or_Empty_Then_Arguments_For_Field_Selection_Are_Empty(
            IArgumentCollection arguments)
        {
            var contactSelection = SelectionSetBuilder.For<Contact>()
                .AddScalarField(contact => contact.FirstName)
                .Build();

            var selectionSet = SelectionSetBuilder.For<Customer>()
                .AddObjectField(customer => customer.CustomerContact, arguments, contactSelection)
                .AddObjectField("foobar", customer => customer.CustomerContact, arguments, contactSelection)
                .Build();

            var expectedContactSelectionSet = new SelectionSet<Contact>(
                new ISelectionSetItem[]
                {
                    new ScalarFieldSelectionItem(null, nameof(Contact.FirstName))
                });

            var expectedSelections = new ISelectionSetItem[]
            {
                new ObjectFieldSelectionItem(null, nameof(Customer.CustomerContact), Enumerable.Empty<IArgument>(), expectedContactSelectionSet),
                new ObjectFieldSelectionItem("foobar", nameof(Customer.CustomerContact), Enumerable.Empty<IArgument>(), expectedContactSelectionSet),
            };

            selectionSet.Should().NotBeNull();
            selectionSet.Selections.Should()
                .BeEquivalentTo(expectedSelections, options => options.RespectingRuntimeTypes());
        }

        [TestCaseSource(nameof(NullOrEmptyArguments))]
        public void If_Arguments_For_Object_Collection_Fields_Are_Null_Or_Empty_Then_Arguments_For_Field_Selection_Are_Empty(
            IArgumentCollection arguments)
        {
            var phoneNumberSelectionSet = SelectionSetBuilder.For<PhoneNumber>()
                .AddScalarField(phone => phone.Number)
                .AddScalarField(phone => phone.Extension)
                .Build();

            var selectionSet = SelectionSetBuilder.For<Contact>()
                .AddObjectCollectionField(contact => contact.PhoneNumbers, arguments, phoneNumberSelectionSet)
                .AddObjectCollectionField("foobar", contact => contact.PhoneNumbers, arguments, phoneNumberSelectionSet)
                .Build();

            var expectedPhoneNumberSelectionSet = new SelectionSet<PhoneNumber>(
                new ISelectionSetItem[]
                {
                    new ScalarFieldSelectionItem(null, nameof(PhoneNumber.Number)),
                    new ScalarFieldSelectionItem(null, nameof(PhoneNumber.Extension))
                });

            var expectedSelections = new ISelectionSetItem[]
            {
                new ObjectFieldSelectionItem(null, nameof(Contact.PhoneNumbers), Enumerable.Empty<IArgument>(), expectedPhoneNumberSelectionSet),
                new ObjectFieldSelectionItem("foobar", nameof(Contact.PhoneNumbers), Enumerable.Empty<IArgument>(), expectedPhoneNumberSelectionSet),
            };

            selectionSet.Should().NotBeNull();
            selectionSet.Selections.Should()
                .BeEquivalentTo(expectedSelections, options => options.RespectingRuntimeTypes());
        }

        [Test]
        public void Then_Arguments_Are_Included_In_Field_Selections()
        {
            var phoneNumberSelection = SelectionSetBuilder.For<PhoneNumber>()
                .AddScalarField(phone => phone.Number)
                .Build();
            var phoneArgs = new ArgumentCollection
            {
                ArgumentBuilder.Build("areaCode", "231")
            };
            var expectedPhoneArgs = new IArgument[phoneArgs.Count];

            // copy arguments so we don't contaminate expectations
            phoneArgs.CopyTo(expectedPhoneArgs,0);

            var contactSelection = SelectionSetBuilder.For<Contact>()
                .AddObjectCollectionField(contact => contact.PhoneNumbers, phoneArgs, phoneNumberSelection)
                .AddObjectCollectionField("foobar", contact => contact.PhoneNumbers, phoneArgs, phoneNumberSelection)
                .Build();

            var numbersArgs = new ArgumentCollection
            {
                ArgumentBuilder.Build("greaterThan", 10),
                ArgumentBuilder.Build("isEven", true)
            };
            var expectedNumbersArgs = new IArgument[numbersArgs.Count];

            // copy arguments so we don't contaminate expectations
            numbersArgs.CopyTo(expectedNumbersArgs, 0);

            var customerSelection = SelectionSetBuilder.For<Customer>()
                .AddScalarCollectionField(customer => customer.FavoriteNumbers, numbersArgs)
                .AddScalarCollectionField("myNumbers", customer => customer.FavoriteNumbers, numbersArgs)
                .AddObjectField(customer => customer.CustomerContact, contactSelection)
                .Build();

            var expectedPhoneSelection = new SelectionSet<PhoneNumber>(new List<ISelectionSetItem>
            {
                new ScalarFieldSelectionItem(null, nameof(PhoneNumber.Number))
            });
            var expectedContactSelection = new SelectionSet<Contact>(new List<ISelectionSetItem>
            {
                new ObjectFieldSelectionItem(null, nameof(Contact.PhoneNumbers), expectedPhoneArgs, expectedPhoneSelection),
                new ObjectFieldSelectionItem("foobar", nameof(Contact.PhoneNumbers), expectedPhoneArgs, expectedPhoneSelection),
            });
            var expectedCustomerSelections = new List<ISelectionSetItem>
            {
                new ScalarFieldSelectionItem(null, nameof(Customer.FavoriteNumbers), expectedNumbersArgs),
                new ScalarFieldSelectionItem("myNumbers", nameof(Customer.FavoriteNumbers), expectedNumbersArgs),
                new ObjectFieldSelectionItem(null, nameof(Customer.CustomerContact), expectedContactSelection)
            };

            customerSelection.Should().NotBeNull();
            customerSelection.Selections.Should()
                .BeEquivalentTo(expectedCustomerSelections, options => options.RespectingRuntimeTypes());
        }

        [Test]
        public void Then_Added_Properties_Are_In_The_Selection_Set()
        {
            var selectionSet = SelectionSetBuilder.For<Customer>()
                .AddScalarField(customer => customer.Id)
                .AddScalarField("acctNum", customer => customer.AccountNumber)
                .Build();

            var expectedSelections = new List<IFieldSelectionItem>
            {
                new ScalarFieldSelectionItem(null, nameof(Customer.Id)),
                new ScalarFieldSelectionItem("acctNum", nameof(Customer.AccountNumber))
            };

            selectionSet.Should().NotBeNull();
            selectionSet.Selections.Should().BeEquivalentTo(expectedSelections, options => options.RespectingRuntimeTypes());
        }

        [Test]
        public void If_Added_Properties_Are_Classes_Then_They_Are_In_The_Selection_Set()
        {
            var contactSelectionSet = SelectionSetBuilder.For<Contact>()
                .AddScalarField(contact => contact.FirstName)
                .AddScalarField(contact => contact.LastName)
                .Build();

            var customerSelectionSet = SelectionSetBuilder.For<Customer>()
                .AddScalarField(customer => customer.Id)
                .AddObjectField(customer => customer.CustomerContact, contactSelectionSet)
                .AddObjectField("foobar", customer => customer.CustomerContact, contactSelectionSet)
                .Build();

            var expectedContactSelectionSet = new SelectionSet<Contact>(
                new ISelectionSetItem[]
                {
                    new ScalarFieldSelectionItem(null, nameof(Contact.FirstName)),
                    new ScalarFieldSelectionItem(null, nameof(Contact.LastName)),
                });
            var expectedCustomerSelections = new List<ISelectionSetItem>
            {
                new ScalarFieldSelectionItem(null, nameof(Customer.Id)),
                new ObjectFieldSelectionItem(null, nameof(Customer.CustomerContact), expectedContactSelectionSet),
                new ObjectFieldSelectionItem("foobar", nameof(Customer.CustomerContact), expectedContactSelectionSet),
            };

            customerSelectionSet.Should().NotBeNull();
            customerSelectionSet.Selections.Should().BeEquivalentTo(expectedCustomerSelections, options => options.RespectingRuntimeTypes());
        }

        [Test]
        public void If_Added_Properties_Are_Collection_Then_They_Are_In_The_Selection_Set()
        {
            var phoneNumberSelectionSet = SelectionSetBuilder.For<PhoneNumber>()
                .AddScalarField(phone => phone.Number)
                .AddScalarField("ext", phone => phone.Extension)
                .Build();

            var contactSelectionSet = SelectionSetBuilder.For<Contact>()
                .AddScalarField(contact => contact.FirstName)
                .AddScalarField("surname", contact => contact.LastName)
                .AddObjectCollectionField(contact => contact.PhoneNumbers, phoneNumberSelectionSet)
                .AddObjectCollectionField("foobar", contact => contact.PhoneNumbers, phoneNumberSelectionSet)
                .AddScalarCollectionField("names", contact => contact.Nicknames)
                .Build();

            var customerSelectionSet = SelectionSetBuilder.For<Customer>()
                .AddScalarField(customer => customer.Id)
                .AddScalarCollectionField(customer => customer.FavoriteNumbers)
                .AddScalarCollectionField("baz", customer => customer.FavoriteNumbers)
                .AddObjectField(customer => customer.CustomerContact, contactSelectionSet)
                .Build();

            var expectedPhoneNumberSelectionSet = new SelectionSet<PhoneNumber>(
                new List<ISelectionSetItem>
                {
                    new ScalarFieldSelectionItem(null, nameof(PhoneNumber.Number)),
                    new ScalarFieldSelectionItem("ext", nameof(PhoneNumber.Extension))
                });

            var expectedContactSelection = new SelectionSet<Contact>(
                new List<ISelectionSetItem>
                {
                    new ScalarFieldSelectionItem(null, nameof(Contact.FirstName)),
                    new ScalarFieldSelectionItem("surname", nameof(Contact.LastName)),
                    new ObjectFieldSelectionItem(null, nameof(Contact.PhoneNumbers), expectedPhoneNumberSelectionSet),
                    new ObjectFieldSelectionItem("foobar", nameof(Contact.PhoneNumbers), expectedPhoneNumberSelectionSet),
                    new ScalarFieldSelectionItem("names", nameof(Contact.Nicknames))
                });

            var expectedCustomerSelections = new List<ISelectionSetItem>
            {
                new ScalarFieldSelectionItem(null, nameof(Customer.Id)),
                new ScalarFieldSelectionItem(null, nameof(Customer.FavoriteNumbers)),
                new ScalarFieldSelectionItem("baz", nameof(Customer.FavoriteNumbers)),
                new ObjectFieldSelectionItem(null, nameof(Customer.CustomerContact), expectedContactSelection)
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