using GraphQLQueryBuilder.Tests.Models;
using NUnit.Framework;

namespace GraphQLQueryBuilder.Tests
{
    public class When_Nesting_Queries : TestClass
    {
        [Test]
        public void If_Nested_Query_Is_Empty_Then_Query_Content_Has_An_Empty_Nested_Query()
        {
            var addressQuery = QueryContentBuilder.Of<Address>();
            var contactQuery = QueryContentBuilder.Of<Contact>()
                .AddField(contact => contact.Address, addressQuery);

            var query = new QueryOperationBuilder()
                .AddField("customer", contactQuery);

            QueryContentShouldMatchSnapshotForTest(query);
        }

        [Test]
        public void If_Nested_Query_Uses_An_Alias_Then_Aliased_Query_Is_Included_In_Query_Content()
        {
            var addressQuery = QueryContentBuilder.Of<Address>();
            var contactQuery = QueryContentBuilder.Of<Contact>()
                .AddField("primaryAddress", contact => contact.Address, addressQuery)
                .AddField("billingAddress", contact => contact.Address, addressQuery);

            var query = new QueryOperationBuilder()
                .AddField("customer", contactQuery);

            QueryContentShouldMatchSnapshotForTest(query);
        }

        [Test]
        public void Then_Nested_Query_Content_Is_Rendered()
        {
            var addressQuery = QueryContentBuilder.Of<Address>()
                .AddField(address => address.Street1)
                .AddField(address => address.Street2)
                .AddField(address => address.City)
                .AddField(address => address.State)
                .AddField(address => address.ZipCode);

            var contactQuery = QueryContentBuilder.Of<Contact>()
                .AddField(contact => contact.FirstName)
                .AddField(contact => contact.LastName)
                .AddField(contact => contact.Address, addressQuery);

            var query = new QueryOperationBuilder()
                .AddField("customer", contactQuery);

            QueryContentShouldMatchSnapshotForTest(query);
        }

        [Test]
        public void If_A_Field_Is_Added_With_A_Nested_Query_Builder_Func_Then_The_Nested_Query_Is_Rendered()
        {
            var customerQuery = QueryContentBuilder.Of<Customer>()
                .AddField(c => c.Id)
                .AddField(c => c.AccountNumber)
                .AddField(
                    c => c.CustomerContact,
                    contactQuery => contactQuery.AddField(c => c.FirstName)
                        .AddField(c => c.LastName)
                );

            var queryBuilder = new QueryOperationBuilder()
                .AddField("customer", customerQuery);

            QueryContentShouldMatchSnapshotForTest(queryBuilder);
        }

        [Test]
        public void If_An_Aliased_Field_Is_Added_With_A_Nested_Query_Builder_Func_Then_Aliased_Nested_Query_Is_Rendered()
        {
            var customerQuery = QueryContentBuilder.Of<Customer>()
                .AddField(customer => customer.Id)
                .AddField(customer => customer.AccountNumber)
                .AddField(
                    customer => customer.CustomerContact,
                    contactQuery => contactQuery.AddField(contact => contact.FirstName)
                        .AddField(contact => contact.LastName)
                );

            var query = new QueryOperationBuilder()
                .AddField("customer", customerQuery);

            QueryContentShouldMatchSnapshotForTest(query);
        }

        [Test]
        public void If_A_Nested_Query_Uses_A_Nested_Query_Then_Both_Nested_Queries_Are_Rendered()
        {
            var customerQuery = QueryContentBuilder.Of<Customer>()
                .AddField(c => c.Id)
                .AddField(c => c.AccountNumber)
                .AddField(
                    c => c.CustomerContact,
                    contactQuery => contactQuery
                        .AddField(contact => contact.FirstName)
                        .AddField(contact => contact.LastName)
                        .AddField(contact => contact.Address,
                            addressQuery => addressQuery
                                .AddField(address => address.Street1)
                                .AddField(address => address.Street2)
                                .AddField(address => address.City)
                                .AddField(address => address.State)
                                .AddField(address => address.ZipCode)
                        )
                );

            var query = new QueryOperationBuilder()
                .AddField("customer", customerQuery);

            QueryContentShouldMatchSnapshotForTest(query);
        }

        [Test]
        public void If_An_Aliased_Nested_Query_Is_Added_Then_Alias_And_Query_Are_Rendered()
        {
            var customerQuery = QueryContentBuilder.Of<Customer>()
                .AddField(customer => customer.Id)
                .AddField(customer => customer.AccountNumber)
                .AddField(
                    "primaryContact",
                    customer => customer.CustomerContact,
                    contactQuery => contactQuery.AddField(contact => contact.FirstName)
                        .AddField("surname", contact => contact.LastName)
                );
            
            var query = new QueryOperationBuilder()
                .AddField("customer", customerQuery);

            QueryContentShouldMatchSnapshotForTest(query);
        }
        
        [Test]
        public void If_A_Nested_Query_For_A_Collection_Field_Is_Added_Then_Nested_Query_Is_Included_In_Content()
        {
            Assert.Fail("TODO");
            //var customerQuery = new QueryBuilder<Customer>("customer")
            //    .AddField(c => c.Id)
            //    .AddField(
            //        c => c.CustomerContact,
            //        contactQuery => contactQuery.AddCollectionField(
            //            contact => contact.PhoneNumbers,
            //            phoneNumberQuery => phoneNumberQuery
            //                .AddField(p => p.Number)
            //                .AddField(p => p.Extension)
            //        )
            //    );

            //var query = new QueryRootBuilder()
            //    .AddQuery(customerQuery)
            //    .Build();

            //ResultMatchesSnapshotOfMatchingClassAndTestName(query);
        }

        [Test]
        public void If_A_Collection_Field_With_An_Alias_Is_Added_Then_Nested_Query_Is_Included_In_Content()
        {
            Assert.Fail("TODO");
            //var customerQuery = new QueryBuilder<Customer>("customer")
            //    .AddField(c => c.Id)
            //    .AddField(
            //        c => c.CustomerContact,
            //        contactQuery => contactQuery.AddCollectionField(
            //            "phones",
            //            contact => contact.PhoneNumbers,
            //            phoneNumberQuery => phoneNumberQuery
            //                .AddField(p => p.Number)
            //                .AddField(p => p.Extension)
            //        )
            //    );

            //var query = new QueryRootBuilder()
            //    .AddQuery(customerQuery)
            //    .Build();

            //ResultMatchesSnapshotOfMatchingClassAndTestName(query);
        }
    }
}