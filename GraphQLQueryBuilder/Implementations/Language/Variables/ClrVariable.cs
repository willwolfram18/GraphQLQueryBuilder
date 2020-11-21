using System;

namespace GraphQLQueryBuilder.Implementations.Language
{
    internal class ClrVariable : Variable
    {
        public ClrVariable(string name, Type clrType) : base(name, null)
        {
            ClrType = clrType;
        }

        /// <inheritdoc />
        public Type ClrType { get; }
    }
}