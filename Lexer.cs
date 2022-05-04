using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JSONParser
{

  public enum TokenType
  {
    OPENING_CURLY_BRACE,
    CLOSING_CURLY_BRACE,
    OPENING_BRACKET,
    CLOSING_BRACKET,
    COMMA,
    COLON,
    STRING,
    NUMBER,
    INVALID,
    EOF
  }

  public class Token
  {
    private TokenType _tokenType;
    public TokenType Type => _tokenType;
    private String _value;
    public String Value => _value;
    public Token(TokenType tokenType, String value = "")
    {
      _tokenType = tokenType;
      _value = value;
    }
    public override string ToString()
    {
      return $"{_tokenType} {_value}";
    }
  }
  class Lexer
  {
    private string _fileName;
    private string _text;
    public string FileName => _fileName;
    private int _index;
    private char Current => current();
    public Lexer(string fileName)
    {
      _fileName = fileName;
      _text = File.ReadAllText(fileName);
      _index = 0;
    }

    private IEnumerable<Token> _tokens()
    {
      while (true)
      {
        char ch = current();
        if (ch == '\0')
        {
          yield return new Token(TokenType.EOF);
          break;
        }
        if (Char.IsWhiteSpace(ch) || ch == '"')
        {
          next();
        }
        else if (Char.IsLetter(ch))
        {
          int start = _index;
          while (Char.IsLetterOrDigit(Current))
          {
            next();
          }
          yield return new Token(TokenType.STRING, _text.Substring(start, _index - start));
        }
        else if (Char.IsDigit(ch))
        {
          int start = _index;
          while (Char.IsDigit(Current))
          {
            next();
          }
          yield return new Token(TokenType.NUMBER, _text.Substring(start, _index - start));
        }
        else
        {

          yield return ch switch
          {
            ',' => new Token(TokenType.COMMA),
            ':' => new Token(TokenType.COLON),
            '{' => new Token(TokenType.OPENING_CURLY_BRACE),
            '}' => new Token(TokenType.CLOSING_CURLY_BRACE),
            '[' => new Token(TokenType.OPENING_BRACKET),
            ']' => new Token(TokenType.CLOSING_BRACKET),
            _ => new Token(TokenType.INVALID)
          };
          next();
        }
      }
    }
    public List<Token> Tokens => _tokens().ToList<Token>();
    private void next()
    {
      _index++;
    }
    private char current()
    {
      if (_index < _text.Length)
        return _text[_index];
      else
        return '\0';
    }
  }
}