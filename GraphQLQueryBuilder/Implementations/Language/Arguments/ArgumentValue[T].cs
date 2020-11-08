using GraphQLQueryBuilder.Abstractions.Language;

namespace GraphQLQueryBuilder.Implementations.Language
{
    public abstract class ArgumentValue<T> : IArgumentValue
    {
        protected ArgumentValue(T value)
        {
            Value = value;
        }
        
        public T Value { get; }
    }
}