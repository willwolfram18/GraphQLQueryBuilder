using GraphQLQueryBuilder.Abstractions.Language;

namespace GraphQLQueryBuilder
{
    public interface IQueryRenderer
    {
        string Render(ISelectionSet selectionSet);

        string Render(IGraphQLOperation query);
    }
}