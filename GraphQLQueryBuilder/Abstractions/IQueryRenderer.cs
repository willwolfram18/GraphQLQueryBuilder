using GraphQLQueryBuilder.Abstractions.Language;

namespace GraphQLQueryBuilder
{
    public interface IQueryRenderer
    {
        string Render(IGraphQLOperation query);
    }
}