using GraphQLQueryBuilder.Abstractions.Language;

namespace GraphQLQueryBuilder.Implementations.Language
{
    public class StringArgumentValue : ArgumentValue<string>, IStringArgumentValue
    {
        public StringArgumentValue(string value) : base(value)
        {
        }
    }
}