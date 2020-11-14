using System;

namespace GraphQLQueryBuilder.Abstractions.Language
{
    public interface IVariable
    {
        string Name { get; }
        
        GraphQLType Type { get; }
    }
}