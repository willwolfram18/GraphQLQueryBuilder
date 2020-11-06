using System.Collections.Generic;

namespace GraphQLQueryBuilder.Abstractions.Language
{
    public interface ISelectionSet
    {
        IEnumerable<IFieldSelectionItem> Selections { get; }
    }
}