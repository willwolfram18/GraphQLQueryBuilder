using System.Collections.Generic;

namespace GraphQLQueryBuilder.Abstractions.Language
{
    public interface IFieldSelectionItem : ISelectionSetItem
    {
        string Alias { get; }

        string FieldName { get; }
        
        IEnumerable<IArgument> Arguments { get; }

        ISelectionSet SelectionSet { get; }
    }
}