using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphQLQueryBuilder
{
    internal class QueryAppender
    {
        private const char IndentCharacter = ' ';
        private const uint IndentWidth = 4;

        private readonly uint _indentationLevel;
        private readonly StringBuilder _content;
        private readonly string _queryStart;

        internal QueryAppender(string queryName, uint indentationLevel)
        {
            _indentationLevel = indentationLevel;
            _queryStart = BuildIndentation(_indentationLevel) + queryName + " {";
            _content = new StringBuilder();
        }

        internal QueryAppender AppendProperty(string property)
        {
            AppendToContent(property);

            return this;
        }

        internal QueryAppender AppendChildQuery(QueryBuilder query)
        {
            var childQueryContent = query.Build(_indentationLevel + 1);

            AppendToContent(childQueryContent);

            return this;
        }

        internal string Build()
        {
            var indentation = BuildIndentation(_indentationLevel);
            var result = new StringBuilder()
                .AppendLine(_queryStart);

            if (_content.Length != 0)
            {
                result.AppendLine(_content.ToString());
            }

            result.Append(indentation + "}");

            return result.ToString();
        }

        private void AppendToContent(string content)
        {
            if (_content.Length != 0)
            {
                _content.Append(",\n");
            }

            var indentation = BuildIndentation(_indentationLevel + 1);
            if (!content.StartsWith(indentation))
            {
                content = indentation + content;
            }

            _content.Append(content);
        }

        private string BuildIndentation(uint indentationLevel)
        {
            return new string(IndentCharacter, (int)(indentationLevel * IndentWidth));
        }
    }
}