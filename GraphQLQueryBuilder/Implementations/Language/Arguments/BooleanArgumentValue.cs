using GraphQLQueryBuilder.Abstractions.Language;

namespace GraphQLQueryBuilder.Implementations.Language
{
    public class BooleanArgumentValue : ArgumentValue<bool>, IBooleanArgumentValue
    {
        public BooleanArgumentValue(bool value) : base(value)
        {
        }
    }
}