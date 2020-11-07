using GraphQLQueryBuilder.Tests.Models;
using NUnit.Framework;
using Snapshooter.NUnit;

namespace GraphQLQueryBuilder.Tests.QueryRendererTests
{
    public class When_Rendering_A_Query_Operation
    {
        [Test]
        public void Then_Added_Fields_Are_Rendered(
            [Values] GraphQLOperationType operationType,
            [Values(null, "", "   ", "MyQuery")] string operationName)
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

            var customerSelectionSet = SelectionSetBuilder.For<Customer>()
                .AddScalarField(customer => customer.Id)
                .AddScalarField("acctNum", customer => customer.AccountNumber)
                .AddObjectField("contactInfo", customer => customer.CustomerContact, contactSelectionSet)
                .AddScalarCollectionField(customer => customer.FavoriteNumbers)
                .Build();

            var queryOperation = QueryOperationBuilder.ForSchema<SimpleSchema>(operationType, operationName)
                .AddScalarField(schema => schema.Version)
                .AddScalarCollectionField("versions", schema => schema.PastVersions)
                .AddObjectCollectionField("users", schema => schema.Customers, customerSelectionSet)
                .AddObjectField("admin", schema => schema.Administrator, contactSelectionSet)
                .Build();
            
            var result = new QueryRenderer().Render(queryOperation);

            Snapshot.Match(result);
        }
    }
}