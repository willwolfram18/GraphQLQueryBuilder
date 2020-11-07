using System.Collections.Generic;

namespace GraphQLQueryBuilder.Tests.Models
{
    public class SimpleSchema
    {
        public string Version { get; set; }
        
        public Contact Administrator { get; set; }
        
        public ICollection<Customer> Customers { get; set; }
    }
}