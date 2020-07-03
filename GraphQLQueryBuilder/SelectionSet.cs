using System.Text;

namespace GraphQLQueryBuilder
{
    internal class SelectionSet : ISelectionSetWithSettings
    {
        private readonly string _alias;
        private readonly string _field;
        private readonly ISelectionSet _selectionSet;
        private QuerySerializerSettings _settings;

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

        public ISelectionSetWithSettings UpdateSettings(QuerySerializerSettings settings)
        {
            var newSelectionSet = new SelectionSet(_alias, _field, _selectionSet)
            {
                _settings = settings
            };

            return newSelectionSet;
        }

        public string Build()
        {
            var content = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(_alias))
            {
                content.Append($"{_alias}: ");
            }

            content.Append(_field);

            // TODO indent
            switch (_selectionSet)
            {
                case null:
                    // nothing to do/append
                    break;
                case IFragmentContentBuilder fragment:
                    content.AppendLine(" {");
                    
                    var fragmentSpreadIndentation = _settings?.IncreaseIndent().CreateIndentation();

                    content.Append(fragmentSpreadIndentation);
                    content.AppendLine($"...{fragment.Name}");
                    content.Append(_settings?.CreateIndentation());
                    content.Append("}");
                    
                    break;
                case ISelectionSetWithSettings selectionSetWithSettings:
                    selectionSetWithSettings = selectionSetWithSettings.UpdateSettings(_settings);

                    content.Append(" " + selectionSetWithSettings.Build());
                    break;
                default:
                    content.Append(" " + _selectionSet.Build());
                    break;
            }

            return content.ToString();
        }
    }
}