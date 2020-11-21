using System;

namespace GraphQLQueryBuilder.Abstractions.Language
{
    public interface IVariable
    {
        string Name { get; }
        
        IVariableType Type { get; }
    }
}