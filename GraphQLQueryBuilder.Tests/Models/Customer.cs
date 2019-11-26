using System;

namespace GraphQLQueryBuilder.Tests.Models
{
    public class Customer
    {
        public Guid Id { get; set; }

        public string AccountNumber { get; set; }

        public Contact CustomerContact { get; set; }
    }
}