using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace GraphQLQueryBuilder
{
    public abstract class QueryBuilder
    {
        protected QueryBuilder()
        {
        }

        protected QueryBuilder(string name)
        {
            QueryName = name;
        }

        protected List<ChildQuery> ChildQueries { get; } = new List<ChildQuery>();

        protected Dictionary<string, FragmentBuilder> FragmentDefinitions { get; } =
            new Dictionary<string, FragmentBuilder>();

        public string QueryName { get; }

        public string Build()
        {
            return Build(0);
        }

        protected void AddChildQuery(QueryBuilder query)
        {
            AddChildQuery(string.Empty, query);
        }

        protected void AddChildQuery(string alias, QueryBuilder query)
        {
            ChildQueries.Add(new ChildQuery(alias, query));

            MergeFragmentDefinitions(query.FragmentDefinitions);
        }

        protected void AddFragmentDefinition(FragmentBuilder fragment)
        {
            if (FragmentDefinitions.ContainsKey(fragment.Name))
            {
                // TODO message
                throw new InvalidOperationException();
            }
            
            FragmentDefinitions.Add(fragment.Name, fragment);
        }

        private void MergeFragmentDefinitions(Dictionary<string, FragmentBuilder> fragments)
        {
            foreach (var fragment in fragments)
            {
                if (!FragmentDefinitions.ContainsKey(fragment.Key))
                {
                    FragmentDefinitions.Add(fragment.Key, fragment.Value);
                }
            }
        }

        internal abstract string Build(uint indentationLevel);
    }
}