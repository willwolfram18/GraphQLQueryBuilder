using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQLQueryBuilder.Tests.Models;

namespace GraphQLQueryBuilder.Tests
{
    public static class AddressQueries
    {
        public static QueryBuilder<Address> CreateCompleteAddressQueryWithoutFragment()
        {
            var addressQuery = new QueryBuilder<Address>("address");

            addressQuery = AddAllAddressPropertiesToQueryBuilder(addressQuery);

            return addressQuery;
        }

        public static QueryBuilder<Address> AddAllAddressPropertiesToQueryBuilder(QueryBuilder<Address> addressQuery)
        {
            addressQuery.AddField(address => address.Street1)
                .AddField(address => address.Street2)
                .AddField(address => address.City)
                .AddField(address => address.State)
                .AddField(address => address.ZipCode);

            return addressQuery;
        }

        public static FragmentBuilder<Address> CreateCompleteAddressFragment()
        {
            return CreateNamedCompleteAddressFragment("CompleteAddress");
        }

        public static FragmentBuilder<Address> CreateNamedCompleteAddressFragment(string name)
        {
            var addressFragment = new FragmentBuilder<Address>(name)
                .AddField(address => address.Street1)
                .AddField(address => address.Street2)
                .AddField(address => address.City)
                .AddField(address => address.State)
                .AddField(address => address.ZipCode);
            return addressFragment;
        }
    }
}
