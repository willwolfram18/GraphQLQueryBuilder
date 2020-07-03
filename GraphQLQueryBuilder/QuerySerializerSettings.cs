namespace GraphQLQueryBuilder
{
    public class QuerySerializerSettings
    {
        public QuerySerializerSettings(int indent)
        {
            Indent = indent;
        }
        
        public int Indent { get; }

        public QuerySerializerSettings IncreaseIndentBy(int size)
        {
            return new QuerySerializerSettings(Indent + size);
        }

        public string CreateIndentation()
        {
            return new string(' ', Indent);
        }
    }
}