using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CompilerTest.Compiling.Tokenizing
{
    internal class Tokenizer : ITokenizer
    {
        private Dictionary<char, TokenType> knownCharacters = new Dictionary<char, TokenType>()
        {
            { '=', TokenType.Equals },
            { ':', TokenType.Colon },
            { '+', TokenType.Plus },
            { '-', TokenType.Minus },
            { '(', TokenType.LeftBracket },
            { ')', TokenType.RightBracket },
            { '{', TokenType.LeftCurlyBracket },
            { '}', TokenType.RightCurlyBracket },
            { '<', TokenType.Smaller },
            { '>', TokenType.Bigger },
            { ',', TokenType.Comma }
        };

        private List<string> keywords = new List<string>()
        {
            "if",
            "while",
            "input",
            "output",
            "halt"
        };

        public Token[] Tokenize(string code)
        {
            var current = 0;
            var line = 1;
            var tokens = new List<Token>();

            while (current < code.Length)
            {
                var character = code[current];

                switch (character)
                {
                    // NewLine
                    case '\n':
                        line++;
                        break;

                    case '\r':
                        break;

                    // Empty Space
                    case '\t':
                    case ' ':
                        break;

                    // Comment
                    case '/':
                        if (code.Length > current + 1 && code[current + 1] == '/')
                        {
                            current += 2;

                            while (current <= code.Length && code[current] != '\n')
                                current++;
                        }
                        continue;

                    // Known Characters
                    case var known when knownCharacters.ContainsKey(known):
                        tokens.Add(new Token(knownCharacters[known], known, line));
                        break;

                    // Number
                    case var number when Regex.IsMatch(number.ToString(), @"\d"):
                        var numberValue = "";
                        while (Regex.IsMatch(character.ToString(), @"\d"))
                        {
                            numberValue += character;
                            current++;
                            character = code[current];
                        }

                        tokens.Add(new Token(TokenType.Number, numberValue, line));
                        continue;

                    // Text
                    case var number when Regex.IsMatch(number.ToString(), @"\w"):
                        var identifierValue = "";
                        while (Regex.IsMatch(character.ToString(), @"\w"))
                        {
                            identifierValue += character;
                            current++;
                            character = code[current];
                        }
                        if (keywords.Contains(identifierValue))
                            tokens.Add(new Token(TokenType.KeyWord, identifierValue.ToLower(), line));
                        else
                            tokens.Add(new Token(TokenType.Identifier, identifierValue.ToLower(), line));
                        continue;

                    // Unknown
                    default:
                        throw new Exception("Unknown token '" + character + "'");
                }

                current++;
            }

            return tokens.ToArray();
        }
    }
}
