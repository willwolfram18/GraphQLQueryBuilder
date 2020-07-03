using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQLQueryBuilder
{
    internal class QueryContentAppender
    {
        private readonly StringBuilder _content = new StringBuilder();
        private QuerySerializerSettings _settings;

        public QueryContentAppender(QuerySerializerSettings settings)
        {
            _settings = settings;
        }

        private SelectionSetScope StartNewSelectionSet()
        {
            return new SelectionSetScope(this);
        }

        public QueryContentAppender Append(string value)
        {
            _content.Append(value);

            return this;
        }

        public QueryContentAppender Append(IEnumerable<ISelectionSet> selectionSets)
        {
            using (StartNewSelectionSet())
            {
                var contentToAppend = new StringBuilder();

                foreach (var selectionSet in selectionSets)
                {
                    if (contentToAppend.Length != 0)
                    {
                        contentToAppend.AppendLine(",");
                    }

                    contentToAppend.Append(_settings?.CreateIndentation());

                    if (selectionSet is ISelectionSetWithSettings selectionSetWithSettings)
                    {
                        selectionSetWithSettings = selectionSetWithSettings.UpdateSettings(_settings);

                        contentToAppend.Append(selectionSetWithSettings.Build());
                    }
                    else
                    {
                        contentToAppend.Append(selectionSet.Build());
                    }
                }

                if (contentToAppend.Length != 0)
                {
                    _content.AppendLine(contentToAppend.ToString());
                }

                return this;
            }
        }

        public override string ToString()
        {
            return _content.ToString();
        }

        private void AppendLine(string value)
        {
            _content.AppendLine(value);
        }

        private void IncreaseIndentation()
        {
            _settings = _settings?.IncreaseIndent();
        }

        private void DecreaseIndentation()
        {
            _settings = _settings?.DecreaseIndent();
        }

        private class SelectionSetScope : IDisposable
        {
            private readonly QueryContentAppender _appender;

            internal SelectionSetScope(QueryContentAppender appender)
            {
                _appender = appender;

                _appender.AppendLine("{");
                _appender.IncreaseIndentation();
            }

            public void Dispose()
            {
                _appender.DecreaseIndentation();
                
                _appender.Append(_appender._settings?.CreateIndentation());
                _appender.Append("}");
            }
        }
    }
}
