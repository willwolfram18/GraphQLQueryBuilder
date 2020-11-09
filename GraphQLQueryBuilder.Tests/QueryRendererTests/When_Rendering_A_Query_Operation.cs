using GraphQLQueryBuilder.Implementations.Language;
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

        [Test]
        public void Then_Field_Arguments_Are_Rendered(
            [Values] GraphQLOperationType operationType,
            [Values(null, "", "   ", "MyQuery")] string operationName)
        {
            var phoneNumberSelectionSet = SelectionSetBuilder.For<PhoneNumber>()
                .AddScalarField(phone => phone.Number)
                .Build();

            var phoneArguments = new ArgumentCollection
            {
                ArgumentBuilder.Build("areaCode", "231"),
                ArgumentBuilder.Build("foobar")
            };


            var contactSelectionSet = SelectionSetBuilder.For<Contact>()
                .AddObjectCollectionField("michiganNumbers", contact => contact.PhoneNumbers, phoneArguments,
                    phoneNumberSelectionSet)
                .Build();
                
            var numberArguments = new ArgumentCollection
            {
                ArgumentBuilder.Build("isEven", true),
                ArgumentBuilder.Build("greaterThan", 22)
            };

            var customerSelectionSet = SelectionSetBuilder.For<Customer>()
                .AddScalarCollectionField("favEvenNumbersGreaterThan22", customer => customer.FavoriteNumbers,
                    numberArguments)
                .Build();

            var customersArguments = new ArgumentCollection
            {
                ArgumentBuilder.Build("isActive", false),
                ArgumentBuilder.Build("lastName", "Smith"),
                ArgumentBuilder.Build("minRating", 3.2)
            };

            Assert.Fail("TODO: need to call the methods with arg parameters");
//            var query = QueryOperationBuilder.ForSchema<SimpleSchema>()
//                .AddObjectCollectionField(schema => schema)
        }
    }
}