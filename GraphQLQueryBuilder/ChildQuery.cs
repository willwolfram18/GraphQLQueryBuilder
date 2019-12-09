using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQLQueryBuilder
{
    public class ChildQuery
    {
        internal ChildQuery(string alias, QueryBuilder query)
        {
            Query = query;
            Alias = alias;
        }

        internal QueryBuilder Query { get; }

        internal  string Alias { get; }
    }
}
