using System.IO;
using FluentAssertions;
using GraphQLQueryBuilder.Tests.Models;
using NUnit.Framework;
using Snapshooter.NUnit;

namespace GraphQLQueryBuilder.Tests.QueryRendererTests
{
    public class When_Rendering_A_Selection_Set
    {
        [Test]
        public void Then_Specified_Properties_Are_Rendered()
        {
            var addressSelectionSet = SelectionSetBuilder.For<Address>()
                .AddField("line1", address => address.Street1)
                .AddField("line2", address => address.Street2)
                .AddField(address => address.City)
                .AddField(address => address.State)
                .AddField(address => address.ZipCode)
                .Build();

            var phoneNumberSelectionSet = SelectionSetBuilder.For<PhoneNumber>()
                .AddField(phone => phone.Number)
                .AddField("ext", phone => phone.Extension)
                .Build();
            
            var contactSelectionSet = SelectionSetBuilder.For<Contact>()
                .AddField(contact => contact.FirstName)
                .AddField("surname", contact => contact.LastName)
                .AddCollectionField("names", contact => contact.Nicknames)
                .AddField(contact => contact.Address, addressSelectionSet)
                .AddCollectionField(contact => contact.PhoneNumbers, phoneNumberSelectionSet)
                .AddCollectionField("foobar", contact => contact.PhoneNumbers, phoneNumberSelectionSet)
                .Build();

            var selectionSet = SelectionSetBuilder.For<Customer>()
                .AddField(customer => customer.Id)
                .AddField("acctNum", customer => customer.AccountNumber)
                .AddField("contactInfo", customer => customer.CustomerContact, contactSelectionSet)
                .AddCollectionField(customer => customer.FavoriteNumbers)
                .Build();

            IQueryRenderer renderer = new QueryRenderer();

            var result = renderer.Render(selectionSet);

            Snapshot.Match(result);
        }
    }
}
