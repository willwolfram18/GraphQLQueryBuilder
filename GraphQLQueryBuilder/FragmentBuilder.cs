using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQLQueryBuilder
{
    public abstract class FragmentBuilder
    {
        protected FragmentBuilder(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public abstract string Build();
    }
}
