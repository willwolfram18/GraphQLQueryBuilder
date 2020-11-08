using GraphQLQueryBuilder.Abstractions.Language;

namespace GraphQLQueryBuilder.Implementations.Language
{
    public class IntegerArgumentValue : ArgumentValue<int>, IIntegerArgumentValue
    {
        public IntegerArgumentValue(int value) : base(value)
        {
        }
    }
}