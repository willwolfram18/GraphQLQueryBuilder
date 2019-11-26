using System.Collections.Generic;

namespace GraphQLQueryBuilder.Tests.Models
{
    public class Contact
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Address Address { get; set; }

        public ICollection<PhoneNumber> PhoneNumbers { get; set; }
    }
}