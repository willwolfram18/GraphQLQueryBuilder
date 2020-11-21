using System;
using GraphQLQueryBuilder.Abstractions.Language;
using GraphQLQueryBuilder.Guards;

namespace GraphQLQueryBuilder.Implementations.Language
{
    public class Variable : IVariable
    {
        public Variable(string name, IVariableType type)
        {
            Name = name.MustBeValidGraphQLName(nameof(name));
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }
        
        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public IVariableType Type { get; }
    }
}