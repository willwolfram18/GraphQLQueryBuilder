namespace GraphQLQueryBuilder
{
    public class QueryRenderingContext
    {
        private const int DefaultIndentationLevel = 0;

        public QueryRenderingContext() : this(2, ' ')
        {
        }

        public QueryRenderingContext(int indentationSize, char indentationCharacter) : this(indentationSize, indentationCharacter, DefaultIndentationLevel)
        {
        }

        private QueryRenderingContext(int indentationSize, char indentationCharacter, int indentationLevel)
        {
            IndentationSize = indentationSize;
            IndentationCharacter = indentationCharacter;
            IndentationLevel = indentationLevel;
        }

        public int IndentationSize { get; }

        public char IndentationCharacter { get; }

        private int IndentationLevel { get; }

        public string RenderIndentationString()
        {
            return new string(IndentationCharacter, IndentationSize * IndentationLevel);
        }

        public QueryRenderingContext IncreaseIndentationLevel()
        {
            if (IndentationLevel == int.MaxValue)
            {
                return this;
            }

            return new QueryRenderingContext(IndentationSize, IndentationCharacter, IndentationLevel + 1);
        }

        public QueryRenderingContext DecreaseIndentationLevel()
        {
            if (IndentationLevel == DefaultIndentationLevel)
            {
                return this;
            }

            return new QueryRenderingContext(IndentationSize, IndentationCharacter, IndentationLevel - 1);
        }
    }
}
