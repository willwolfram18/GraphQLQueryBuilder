using System;
using GraphQLQueryBuilder.Abstractions.Language;

namespace GraphQLQueryBuilder.Implementations.Language
{
    internal class ClrVariable : IVariable
    {
        public ClrVariable(string name, Type clrType)
        {
            Name = name;
            ClrType = clrType;
        }
        
        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public Type ClrType { get; }

        /// <inheritdoc />
        public GraphQLType Type => ClrType.ToGraphQLType();
    }
}