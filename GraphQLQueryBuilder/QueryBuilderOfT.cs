using System;
using System.Linq.Expressions;

namespace GraphQLQueryBuilder
{
    public class QueryBuilder<T> where T : class
    {
        public QueryBuilder<T> AddQuery(string queryName) {
            throw new NotImplementedException();
        }

        public QueryBuilder<T> AddProperty<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            throw new NotImplementedException();
        }

        public string Build()
        {
            return null;
        }
    }
}