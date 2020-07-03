using System.Text;

namespace GraphQLQueryBuilder
{
    internal class SelectionSet : ISelectionSet
    {
        private readonly string _alias;
        private readonly string _field;
        private readonly ISelectionSet _selectionSet;

        private SelectionSet(string alias, string field, ISelectionSet selectionSet)
        {
            _alias = alias;
            _field = field;
            _selectionSet = selectionSet;
        }

        public static ISelectionSet Create(string field)
        {
            return new SelectionSet(string.Empty, field, null);
        }

        public static ISelectionSet Create(string alias, string field)
        {
            return new SelectionSet(alias, field, null);
        }

        public static ISelectionSet Create(string field, ISelectionSet selectionSet)
        {
            return new SelectionSet(string.Empty, field, selectionSet);
        }

        public static ISelectionSet Create(string alias, string field, ISelectionSet selectionSet)
        {
            return new SelectionSet(alias, field, selectionSet);
        }

        public string Build()
        {
            var content = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(_alias))
            {
                content.Append($"{_alias}: ");
            }

            content.Append(_field);

            if (_selectionSet != null)
            {
                // TODO indent
                content.Append(" " + _selectionSet.Build());
            }

            return content.ToString();
        }
    }
}