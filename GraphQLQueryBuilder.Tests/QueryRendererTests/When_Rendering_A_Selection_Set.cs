﻿using GraphQLQueryBuilder.Implementations.Language;
using GraphQLQueryBuilder.Tests.Models;
using NUnit.Framework;
using Snapshooter;
using Snapshooter.NUnit;
using System.Runtime.InteropServices;

namespace GraphQLQueryBuilder.Tests.QueryRendererTests
{
    public class When_Rendering_A_Selection_Set
    {
        [Test]
        public void Then_Specified_Properties_Are_Rendered()
        {
            var addressSelectionSet = SelectionSetBuilder.For<Address>()
                .AddScalarField("line1", address => address.Street1)
                .AddScalarField("line2", address => address.Street2)
                .AddScalarField(address => address.City)
                .AddScalarField(address => address.State)
                .AddScalarField(address => address.ZipCode)
                .Build();

            var phoneNumberSelectionSet = SelectionSetBuilder.For<PhoneNumber>()
                .AddScalarField(phone => phone.Number)
                .AddScalarField("ext", phone => phone.Extension)
                .Build();

            var contactSelectionSet = SelectionSetBuilder.For<Contact>()
                .AddScalarField(contact => contact.FirstName)
                .AddScalarField("surname", contact => contact.LastName)
                .AddScalarCollectionField("names", contact => contact.Nicknames)
                .AddObjectField(contact => contact.Address, addressSelectionSet)
                .AddObjectCollectionField(contact => contact.PhoneNumbers, phoneNumberSelectionSet)
                .AddObjectCollectionField("foobar", contact => contact.PhoneNumbers, phoneNumberSelectionSet)
                .Build();

            var selectionSet = SelectionSetBuilder.For<Customer>()
                .AddScalarField(customer => customer.Id)
                .AddScalarField("acctNum", customer => customer.AccountNumber)
                .AddObjectField("contactInfo", customer => customer.CustomerContact, contactSelectionSet)
                .AddScalarCollectionField(customer => customer.FavoriteNumbers)
                .Build();

            IQueryRenderer renderer = new QueryRenderer();

            var result = renderer.Render(selectionSet);

            Snapshot.Match(result);
        }

        [Test]
        public void Then_Arguments_Are_Rendered_Properly()
        {
            var addressSelectionSet = SelectionSetBuilder.For<Address>()
                .AddScalarField("postalCode", address => address.ZipCode)
                .Build();
            var phoneSelectionSet = SelectionSetBuilder.For<PhoneNumber>()
                .AddScalarField(phone => phone.Number)
                .Build();

            var nameArguments = new ArgumentCollection
            {
                ArgumentBuilder.Build("initials", true),
                ArgumentBuilder.Build("length", 3),
                ArgumentBuilder.Build("filter", (string) null)
            };
            var addressArguments = new ArgumentCollection
            {
                ArgumentBuilder.Build("state", "CA"),
                ArgumentBuilder.Build("withApartments", false),
                ArgumentBuilder.Build("distanceFromDc", 300.25)
            };
            var phoneNumberArguments = new ArgumentCollection
            {
                ArgumentBuilder.Build("areaCode", "231"),
                ArgumentBuilder.Build("foobar")
            };

            var contactSelectionSet = SelectionSetBuilder.For<Contact>()
                .AddScalarField(contact => contact.FirstName, nameArguments)
                .AddObjectField(contact => contact.Address, addressArguments, addressSelectionSet)
                .AddObjectCollectionField("phones", contact => contact.PhoneNumbers, phoneNumberArguments, phoneSelectionSet)
                .Build();

            var contactArguments = new ArgumentCollection
            {
                ArgumentBuilder.Build("isActive", true)
            };
            var favoriteNumberArguments = new ArgumentCollection
            {
                ArgumentBuilder.Build("isEven", false),
                ArgumentBuilder.Build("greaterThan", 10)
            };

            var customerSelectionSet = SelectionSetBuilder.For<Customer>()
                .AddObjectField(customer => customer.CustomerContact, contactArguments, contactSelectionSet)
                .AddScalarCollectionField("favoriteOddNumbersGreaterThan10", customer => customer.FavoriteNumbers, favoriteNumberArguments)
                .Build();

            var result = new QueryRenderer().Render(customerSelectionSet);

            Snapshot.Match(result);
        }

        [Test]
        public void Then_String_Argument_Values_Are_Serialized()
        {
            var argumentValue =
                @"Lorem ipsum dolor sit amet, ""consectetur adipiscing"" elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
    Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in 'voluptate' velit esse cillum dolore eu fugiat nulla pariatur.

    Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

            var argument = ArgumentBuilder.Build("description", argumentValue);

            var selectionSet = SelectionSetBuilder.For<Customer>()
                .AddScalarField(customer => customer.Id, new ArgumentCollection {argument})
                .Build();

            var result = new QueryRenderer().Render(selectionSet);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Snapshot.Match(result, new SnapshotNameExtension("Windows"));
            }
            else
            {
                Snapshot.Match(result);
            }
        }
    }
}
