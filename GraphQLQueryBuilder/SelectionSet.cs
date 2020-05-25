using System.Text;

namespace GraphQLQueryBuilder
{
    internal class SelectionSet : ISelectionSet
    {
        private readonly string _alias;
        private readonly string _field;
        private readonly ISelectionSet _selectionSet;

        private SelectionSet(string alias, string field, ISelectionSet fragment)
        {
            _alias = alias;
            _field = field;
            _selectionSet = fragment;
        }

        public static ISelectionSet Create(string field)
        {
            return new SelectionSet(string.Empty, field, null);
        }

        public static ISelectionSet Create(string alias, string field)
        {
            return new SelectionSet(alias, field, null);
        }

        public static ISelectionSet Create(string field, ISelectionSet fragment)
        {
            return new SelectionSet(string.Empty, field, fragment);
        }

        public static ISelectionSet Create(string alias, string field, ISelectionSet fragment)
        {
            return new SelectionSet(alias, field, fragment);
        }

        public string Build()
        {
            var content = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(_alias))
            {
                content.Append($"{_alias}: ");
            }

            content.Append(_field);

            if (_selectionSet != null) {
                content.AppendLine(" {");

                // TODO indent
                content.Append(_selectionSet.Build());
                content.Append("}");
            }

            return content.ToString();
        }
    }
}