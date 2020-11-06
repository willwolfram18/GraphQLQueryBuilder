using System.Collections.Generic;

namespace GraphQLQueryBuilder.Abstractions.Language
{
    public interface ISelectionSet<T> : ISelectionSet where T : class
    {
    }
}