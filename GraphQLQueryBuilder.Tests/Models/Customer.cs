using System;
using System.Collections.Generic;

namespace GraphQLQueryBuilder.Tests.Models
{
    public class Customer
    {
        public Guid Id { get; set; }

        public string AccountNumber { get; set; }

        public Contact CustomerContact { get; set; }
        
        public IEnumerable<int> FavoriteNumbers { get; set; }
    }
}