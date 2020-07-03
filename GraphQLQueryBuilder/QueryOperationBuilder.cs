using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace GraphQLQueryBuilder
{
    public class QueryOperationBuilder : IGraphQLQueryContentBuilder, IQueryOperation
    {
        private static readonly Regex GraphQLNameRegex = new Regex(@"^[_A-Za-z][_0-9A-Za-z]*$");

        private readonly QuerySerializerSettings _settings = new QuerySerializerSettings(4);
        private readonly List<ISelectionSet> _selections = new List<ISelectionSet>();
        private readonly Dictionary<string, IFragmentContentBuilder> _fragmentDefinitions = new Dictionary<string, IFragmentContentBuilder>();

        public QueryOperationBuilder()
        {
        }

        public QueryOperationBuilder(string operationName)
        {
            ThrowIfNameIsNotValid(operationName, "The query operation name is not valid.", nameof(operationName));

            OperationName = operationName;
        }

        public string OperationName { get; }

        public GraphQLOperationTypes OperationType => GraphQLOperationTypes.Query;

        public IGraphQLQueryContentBuilder AddField(string field)
        {
            ThrowIfNameIsNotValid(field, "The field name is invalid.", nameof(field));

            _selections.Add(SelectionSet.Create(field));

            return this;
        }

        public IGraphQLQueryContentBuilder AddField(string alias, string field)
        {
            ThrowIfNameIsNotValid(alias, "The alias is invalid.", nameof(alias));
            ThrowIfNameIsNotValid(field, "The field name is invalid.", nameof(field));

            _selections.Add(SelectionSet.Create(alias, field));

            return this;
        }

        public IGraphQLQueryContentBuilder AddField(string field, ISelectionSet selectionSet)
        {
            ThrowIfNameIsNotValid(field, "The field name is invalid.", nameof(field));

            if (selectionSet == null)
            {
                throw new ArgumentNullException(nameof(selectionSet));
            }

            if (selectionSet is IQueryOperation)
            {
                throw new InvalidOperationException("A GraphQL query opteration cannot be added as a selection set.");
            }

            _selections.Add(SelectionSet.Create(field, selectionSet));

            return this;
        }

        public IGraphQLQueryContentBuilder AddField(string alias, string field, ISelectionSet selectionSet)
        {
            ThrowIfNameIsNotValid(alias, "The alias is invalid.", nameof(alias));
            ThrowIfNameIsNotValid(field, "The field name is invalid.", nameof(field));

            if (selectionSet == null)
            {
                throw new ArgumentNullException(nameof(selectionSet));
            }

            if (selectionSet is IQueryOperation)
            {
                throw new InvalidOperationException("A GraphQL query opteration cannot be added as a selection set.");
            }

            _selections.Add(SelectionSet.Create(alias, field, selectionSet));

            return this;
        }

        public IGraphQLQueryContentBuilder AddFragment(IFragmentContentBuilder fragment)
        {
            throw new NotImplementedException();
        }

        public string Build()
        {
            var content = new StringBuilder();

            content.Append($"query ");

            if (!string.IsNullOrWhiteSpace(OperationName))
            {
                content.Append($"{OperationName} ");
            }

            content.AppendLine("{");

            var selectionSet = BuildSelectionSetContent();

            if (!string.IsNullOrWhiteSpace(selectionSet))
            {
                content.AppendLine(selectionSet);
            }

            content.Append("}");

            return content.ToString();
        }

        private static void ThrowIfNameIsNotValid(string value, string message, string argumentName)
        {
            if (string.IsNullOrWhiteSpace(value) || !GraphQLNameRegex.IsMatch(value))
            {
                throw new ArgumentException(message, argumentName);
            }
        }

        private string BuildSelectionSetContent()
        {
            if (_selections.Count == 0)
            {
                return string.Empty;
            }

            var selectionSetContent = new StringBuilder();

            foreach (var selectionSet in _selections)
            {
                if (selectionSetContent.Length != 0)
                {
                    selectionSetContent.AppendLine(",");
                }

                if (selectionSet is ISelectionSetWithSettings selectionSetWithSettings)
                {
                    selectionSetWithSettings = selectionSetWithSettings.UpdateSettings(_settings);

                    selectionSetContent.Append(selectionSetWithSettings.Build());
                }
                else
                {
                    selectionSetContent.Append($"{_settings?.CreateIndentation()}");
                    selectionSetContent.Append(selectionSet.Build());
                }
            }

            return selectionSetContent.ToString();
        }
    }
}