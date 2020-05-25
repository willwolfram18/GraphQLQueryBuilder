using System;
using System.Text.RegularExpressions;

namespace GraphQLQueryBuilder
{
    public class QueryOperationBuilder : IGraphQLQueryContentBuilder, IQueryOperation
    {
        private static readonly Regex GraphQLNameRegex = new Regex(@"^[_A-Za-z][_0-9A-Za-z]*$");

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

            throw new NotImplementedException();
        }

        public IGraphQLQueryContentBuilder AddField(string alias, string field)
        {
            ThrowIfNameIsNotValid(alias, "The alias is invalid.", nameof(alias));
            ThrowIfNameIsNotValid(field, "The field name is invalid.", nameof(field));

            throw new NotImplementedException();
        }

        public IGraphQLQueryContentBuilder AddField(string field, ISelectionSet selectionSet)
        {
            ThrowIfNameIsNotValid(field, "The field name is invalid.", nameof(field));

            if (selectionSet == null)
            {
                throw new ArgumentNullException(nameof(selectionSet));
            }

            throw new NotImplementedException();
        }

        public IGraphQLQueryContentBuilder AddField(string alias, string field, ISelectionSet selectionSet)
        {
            ThrowIfNameIsNotValid(alias, "The alias is invalid.", nameof(alias));
            ThrowIfNameIsNotValid(field, "The field name is invalid.", nameof(field));

            if (selectionSet == null)
            {
                throw new ArgumentNullException(nameof(selectionSet));
            }

            throw new NotImplementedException();
        }

        public IGraphQLQueryContentBuilder AddFragment(IFragmentContentBuilder fragment)
        {
            throw new NotImplementedException();
        }

        public string Build()
        {
            throw new NotImplementedException();
        }

        private static void ThrowIfNameIsNotValid(string value, string message, string argumentName)
        {
            if (string.IsNullOrWhiteSpace(value) || !GraphQLNameRegex.IsMatch(value))
            {
                throw new ArgumentException(message, argumentName);
            }
        }
    }
}