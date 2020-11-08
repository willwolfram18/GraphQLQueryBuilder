using System.Collections.Generic;
using System.Linq;
using GraphQLQueryBuilder.Abstractions.Language;

namespace GraphQLQueryBuilder.Implementations.Language
{
    internal class ScalarFieldSelectionItem : IFieldSelectionItem
    {
        public ScalarFieldSelectionItem(string alias, string fieldName) : this(alias, fieldName, null)
        {
        }
        
        public ScalarFieldSelectionItem(string alias, string fieldName, IEnumerable<IArgument> arguments)
        {
            Alias = alias;
            FieldName = fieldName;
            Arguments = (arguments ?? Enumerable.Empty<IArgument>())
                .Where(e => e != null)
                .ToList()
                .AsReadOnly();
        }
        
        public string Alias { get; }
        
        public string FieldName { get; }
        
        public IReadOnlyCollection<IArgument> Arguments { get; }

        public ISelectionSet SelectionSet => null;
    }
}