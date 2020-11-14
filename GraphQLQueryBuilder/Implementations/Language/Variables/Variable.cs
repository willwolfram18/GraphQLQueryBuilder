using GraphQLQueryBuilder.Abstractions.Language;

namespace GraphQLQueryBuilder.Implementations.Language
{
    public class Variable : IVariable
    {
        public Variable(string name, GraphQLType type)
        {
            Name = name;
            Type = type;
        }
        
        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public GraphQLType Type { get; }
    }
}