using MCCompiler.Compiler.Shared;

namespace MCCompiler.Compiler.Parser.Implementations
{
    public abstract class ParserBase
    {
        protected Token? CurrentToken;

        private int _current;
        private IEnumerable<Token> _tokens;

        public ParserBase()
            => _tokens = new List<Token>();

        protected void InitBase(IEnumerable<Token> tokens)
        {
            _tokens = tokens;
            Advance(0);
        }

        protected static Exception Error(Token token, string message, params object[] values)
            => new ParserException(token, string.Format("Parsing error on line {0}: {1}", token.Line, string.Format(message, values)));

        protected Exception Error(string message, params object[] values)
            => new ParserException(CurrentToken, $"Parsing error: {string.Format(message, values)}");

        protected bool PeekType(int count, params TokenType[] types)
        {
            return _tokens.Count() > _current + count
                && types.Contains(_tokens.ElementAt(_current + count).Type);
        }

        protected void Consume(params TokenType[] types)
        {
            if (CurrentToken is null)
                return; //TODO: Error

            if (!types.Contains(CurrentToken.Type))
                throw Error(CurrentToken, "Expected '{0}', got '{1}' instead", string.Join(" or ", types), CurrentToken.Content);

            Advance();
        }

        protected void Expect(params TokenType[] types)
        {
            if (CurrentToken is null)
                return;

            if (!types.Contains(CurrentToken.Type))
                throw Error(CurrentToken, "Expected '{0}', got '{1}' instead", string.Join(" or ", types), CurrentToken.Content);
        }

        protected void Advance(int count = 1)
        {
            _current += count;

            if (_current == _tokens.Count())
            {
                CurrentToken = null;
                return;
            }

            CurrentToken = _tokens.ElementAt(_current);
        }
    }
}
