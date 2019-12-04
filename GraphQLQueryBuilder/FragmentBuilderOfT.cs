using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GraphQLQueryBuilder
{
    public class FragmentBuilder<T> where T : class
    {
        private readonly List<string> _fields = new List<string>();

        public FragmentBuilder(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public FragmentBuilder<T> AddField<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            if (!(expression.Body is MemberExpression member))
            {
                throw new InvalidOperationException();
            }

            _fields.Add(member.Member.Name);

            return this;
        }

        public string Build()
        {
            var content = new QueryContentAppender($"fragment {Name} on {typeof(T).Name}", 0);

            foreach (var field in _fields)
            {
                content.AppendProperty(field);
            }

            return content.ToString();
        }
    }
}
