using GraphQLQueryBuilder.Abstractions.Language;
using System;
using System.Text;

namespace GraphQLQueryBuilder
{
    public class QueryRenderer : IQueryRenderer
    {
        /// <inheritdoc />
        public string Render(ISelectionSet selectionSet)
        {
            return Render(selectionSet, new QueryRenderingContext());
        }

        /// <inheritdoc />
        public string Render(IGraphQLOperation query)
        {
            return "fake";
            throw new NotImplementedException();
        }

        private string Render(ISelectionSet selectionSet, QueryRenderingContext context)
        {
            var content = new StringBuilder();

            content.AppendLine("{");
            context = context.IncreaseIndentationLevel();

            foreach (var selectionItem in selectionSet.Selections)
            {
                var selectionItemContent = selectionItem switch
                {
                    IFieldSelectionItem fieldSelection => Render(fieldSelection, context),
                    _ => throw new NotImplementedException($"Unable to render selection {selectionItem.GetType().FullName}")
                };

                content.AppendLine($"{context.RenderIndentationString()}{selectionItemContent}");
            }

            context = context.DecreaseIndentationLevel();
            content.Append($"{context.RenderIndentationString()}}}");

            return content.ToString();
        }

        private string Render(IFieldSelectionItem fieldSelection, QueryRenderingContext context)
        {
            var content = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(fieldSelection.Alias))
            {
                content.Append($"{fieldSelection.Alias}: ");
            }

            content.Append(fieldSelection.FieldName);

            if (fieldSelection.SelectionSet != null)
            {
                content.Append($" {Render(fieldSelection.SelectionSet, context)}");
            }

            return content.ToString();
        }
    }
}
