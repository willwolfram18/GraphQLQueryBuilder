using GraphQLQueryBuilder.Abstractions.Language;

namespace GraphQLQueryBuilder.Implementations.Language
{
    public class VariableArgument : Argument, IVariableArgument
    {
        public VariableArgument(string name, IVariable variable) : base(name, (IVariableArgumentValue)null)
        {
            Variable = variable;
        }

        /// <inheritdoc />
        public IVariable Variable { get; }
    }
}