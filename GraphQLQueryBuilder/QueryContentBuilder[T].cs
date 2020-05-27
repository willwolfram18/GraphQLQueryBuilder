namespace GraphQLQueryBuilder
{
    public class QueryContentBuilder<T> : IGraphQLQueryContentBuilder<T>
        where T : class
    {
        public IGraphQLQueryContentBuilder<T> AddField<TProperty>(System.Linq.Expressions.Expression<System.Func<T, TProperty>> propertyExpression)
        {
            throw new System.NotImplementedException();
        }

        public IGraphQLQueryContentBuilder<T> AddField<TProperty>(string alias, System.Linq.Expressions.Expression<System.Func<T, TProperty>> propertyExpression)
        {
            throw new System.NotImplementedException();
        }

        public IGraphQLQueryContentBuilder<T> AddField<TProperty>(System.Linq.Expressions.Expression<System.Func<T, TProperty>> propertyExpression, ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            throw new System.NotImplementedException();
        }

        public IGraphQLQueryContentBuilder<T> AddField<TProperty>(string alias, System.Linq.Expressions.Expression<System.Func<T, TProperty>> propertyExpression, ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            throw new System.NotImplementedException();
        }

        public IGraphQLQueryContentBuilder AddField(string field)
        {
            throw new System.NotImplementedException();
        }

        public IGraphQLQueryContentBuilder AddField(string alias, string field)
        {
            throw new System.NotImplementedException();
        }

        public IGraphQLQueryContentBuilder AddField(string field, ISelectionSet selectionSet)
        {
            throw new System.NotImplementedException();
        }

        public IGraphQLQueryContentBuilder AddField(string alias, string field, ISelectionSet selectionSet)
        {
            throw new System.NotImplementedException();
        }

        public string Build()
        {
            throw new System.NotImplementedException();
        }
    }
}