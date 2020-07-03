namespace GraphQLQueryBuilder
{
    public class QuerySerializerSettings
    {
        public QuerySerializerSettings(int indentSize)
        {
            IndentSize = indentSize;
            CurrentIndent = 0;
        }
        
        public int IndentSize { get; }
        
        public int CurrentIndent { get; private set; }

        public QuerySerializerSettings IncreaseIndent()
        {
            return new QuerySerializerSettings(IndentSize)
            {
                CurrentIndent = CurrentIndent + IndentSize
            };
        }

        public QuerySerializerSettings DecreaseIndent()
        {
            if (IndentSize == 0)
            {
                return new QuerySerializerSettings(IndentSize);
            }
            
            return new QuerySerializerSettings(IndentSize)
            {
                CurrentIndent = CurrentIndent - IndentSize
            };
        }

        public string CreateIndentation()
        {
            return new string(' ', CurrentIndent);
        }
    }
}