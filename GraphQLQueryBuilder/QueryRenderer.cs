﻿using GraphQLQueryBuilder.Abstractions.Language;
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
            var context = new QueryRenderingContext();
            var content = new QueryContentAppender(context);

            content.Append($"{Render(query.Type)} ")
                .Append(RenderOperationName(query.Name))
                .Append(Render(query.SelectionSet, context));

            return content.ToString();
        }

        private string Render(ISelectionSet selectionSet, QueryRenderingContext context)
        {
            var content = new QueryContentAppender(context);

            (_, context) = content.AppendStartOfSelectionSet();

            foreach (var selectionItem in selectionSet.Selections)
            {
                var selectionItemContent = selectionItem switch
                {
                    IFieldSelectionItem fieldSelection => Render(fieldSelection, context),
                    _ => throw new NotImplementedException($"Unable to render selection {selectionItem.GetType().FullName}")
                };

                content.AppendLineWithIndentation($"{selectionItemContent}");
            }

            content.AppendEndOfSelectionSet();

            return content.ToString();
        }

        private string Render(IFieldSelectionItem fieldSelection, QueryRenderingContext context)
        {
            var content = new QueryContentAppender(context);

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

        private static string Render(GraphQLOperationType type)
        {
            return type switch
            {
                GraphQLOperationType.Mutation => "mutation",
                GraphQLOperationType.Query => "query",
                GraphQLOperationType.Subscription => "subscription",
                _ => throw new NotImplementedException($"Unknown query operation type '{type}'.")
            };
        }
        
        private static string RenderOperationName(string queryName)
        {
            return string.IsNullOrWhiteSpace(queryName) ? null : $"{queryName} ";
        }
    }
}
