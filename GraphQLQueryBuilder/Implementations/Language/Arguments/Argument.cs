using System;
using GraphQLQueryBuilder.Abstractions.Language;
using GraphQLQueryBuilder.Guards;

namespace GraphQLQueryBuilder.Implementations.Language
{
    public class Argument : IArgument
    {
        public Argument(string name, IArgumentValue value)
        {
            Name = name.MustBeValidGraphQLName(nameof(name));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
        
        public string Name { get; }
        
        public IArgumentValue Value { get; }
    }
}