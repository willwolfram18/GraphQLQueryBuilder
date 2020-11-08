using GraphQLQueryBuilder.Abstractions.Language;

namespace GraphQLQueryBuilder.Implementations.Language
{
    public class FloatArgumentValue : ArgumentValue<double>, IFloatArgumentValue
    {
        public FloatArgumentValue(double value) : base(value)
        {
        }
    }
}