using System;
using System.Collections.Generic;
using System.Linq;
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
        public static IEnumerable<List<IArgument>> NullOrEmptyArguments => new[]
        {
            null,
            Enumerable.Empty<IArgument>().ToList()
        };
        
        [TestCaseSource(nameof(NullOrEmptyArguments))]
        public void If_Arguments_For_Scalar_Fields_Are_Null_Or_Empty_Then_Arguments_For_Field_Selection_Are_Empty(
            List<IArgument> arguments)
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
        public void If_Arguments_For_Object_Fields_Are_Null_Or_Empty_Then_Arguments_For_Field_Selection_Are_Empty(
            List<IArgument> arguments)
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

        [Test]
        public void Then_Arguments_For_Scalar_Field_Selections_Exclude_Nulls()
        {
            var arguments = new List<IArgument>
            {
                ArgumentBuilder.Build("first", "foo"),
                ArgumentBuilder.Build("second", 3),
                null,
                ArgumentBuilder.Build("fourth", 10.1),
                ArgumentBuilder.Build("fifth")
            };

            var expectedArguments = arguments.Where(a => a != null).ToList();
            
            var selectionSet = SelectionSetBuilder.For<Customer>()
                .AddScalarField(customer => customer.Id, arguments)
                .AddScalarField("foobar", customer => customer.Id, arguments)
                .Build();

            var expectedSelections = new List<ISelectionSetItem>
            {
                new ScalarFieldSelectionItem(null, nameof(Customer.Id), expectedArguments),
                new ScalarFieldSelectionItem("foobar", nameof(Customer.Id), expectedArguments)
            };
            
            selectionSet.Should().NotBeNull();
            selectionSet.Selections.Should()
                .BeEquivalentTo(expectedSelections, options => options.RespectingRuntimeTypes());
        }
        
        [Test]
        public void Then_Arguments_For_Object_Field_Selections_Exclude_Nulls()
        {
            var arguments = new List<IArgument>
            {
                ArgumentBuilder.Build("first", "foo"),
                ArgumentBuilder.Build("second", 3),
                null,
                ArgumentBuilder.Build("fourth", 10.1),
                ArgumentBuilder.Build("fifth")
            };

            var expectedArguments = arguments.Where(a => a != null).ToList();
            
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
                new ObjectFieldSelectionItem(null, nameof(Customer.CustomerContact), expectedArguments, expectedContactSelectionSet), 
                new ObjectFieldSelectionItem("foobar", nameof(Customer.CustomerContact), expectedArguments, expectedContactSelectionSet),
            };
            
            selectionSet.Should().NotBeNull();
            selectionSet.Selections.Should()
                .BeEquivalentTo(expectedSelections, options => options.RespectingRuntimeTypes());
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