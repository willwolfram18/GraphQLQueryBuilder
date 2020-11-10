using System;
using System.Text;

namespace GraphQLQueryBuilder
{
    internal class QueryContentAppender
    {
        private readonly StringBuilder _content = new StringBuilder();
        
        private QueryRenderingContext _context;

        public QueryContentAppender(QueryRenderingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public QueryContentAppender Append(string content)
        {
            _content.Append(content);

            return this;
        }

        public QueryContentAppender AppendLineWithIndentation(string content)
        {
            _content.AppendLine($"{_context.RenderIndentationString()}{content}");
            
            return this;
        }

        public (QueryContentAppender appender, QueryRenderingContext context) AppendStartOfSelectionSet()
        {
            _content.AppendLine("{");
            _context = _context.IncreaseIndentationLevel();

            return (this, _context);
        }

        public QueryContentAppender AppendEndOfSelectionSet()
        {
            _context = _context.DecreaseIndentationLevel();

            return Append($"{_context.RenderIndentationString()}}}");
        }

        /// <inheritdoc />
        public override string ToString() => _content.ToString();
    }
}