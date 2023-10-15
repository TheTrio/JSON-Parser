using System;
using System.Collections.Generic;
using System.IO;

namespace JSONParser
{

  public enum TokenType
  {
    OPENING_CURLY_BRACE,
    CLOSING_CURLY_BRACE,
    OPENING_BRACKET,
    CLOSING_BRACKET,
    QUOTE,
    COMMA,
    COLON,
    STRING,
    NUMBER,
    INVALID,
    TRUE,
    FALSE,
    NULL,
    EOF
  }

  public class Token
  {
    private TokenType _tokenType;
    public TokenType Type => _tokenType;
    private String _value;
    public String Value => _value;
    public Token(TokenType tokenType, String value)
    {
      _tokenType = tokenType;
      _value = value;
    }
    public override string ToString()
    {
      return $"{_tokenType} {_value}";
    }
  }

  public class Lexer
  {
    private string _fileName;
    private string _text;
    public string FileName => _fileName;
    private int _index;
    private List<string> _errors = new List<string>();
    private List<Token> _tokens = new List<Token>();

    public List<Token> Tokens => _tokens;
    private char Current => current();
    private static readonly List<char> ESCAPE_SEQUENCE = new List<char> { '\n', '\t', '\r', '\b', '\f' };


    public char peek(int offset = 1)
    {
      if (_index + offset >= _text.Length)
      {
        return '\0';
      }
      return _text[_index + offset];
    }
    public Lexer(string fileName)
    {
      _fileName = fileName;
      _text = File.ReadAllText(fileName);
      _index = 0;
      parse_tokens();
    }
    public List<string> Errors => _errors;

    private void parse_tokens()
    {
      while (true)
      {
        char ch = Current;
        if (ch == '\0')
        {
          _tokens.Add(new Token(TokenType.EOF, "\0"));
          break;
        }
        else if (ch == '/' && peek() == '/')
        {
          while (Current != '\n' && Current != '\0')
            next();
        }
        else if (ch == '/' && peek() == '*')
        {
          next();
          next();
          while (Current != '*' && peek() != '/' && Current != '\0')
          {
            next();
          }
          next();
          next();
        }
        else if (Char.IsWhiteSpace(ch))
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
          string word = _text.Substring(start, _index - start);
          if (word == "true")
          {
            _tokens.Add(new Token(TokenType.TRUE, word));
          }
          else if (word == "false")
          {
            _tokens.Add(new Token(TokenType.FALSE, word));
          }
          else if (word == "null")
          {
            _tokens.Add(new Token(TokenType.NULL, word));
          }
          else
          {
            _tokens.Add(new Token(TokenType.INVALID, word));
            _errors.Add($"Unexpected keyword {word}");
          }
        }
        else if (Char.IsDigit(ch) || ch == '-')
        {
          int start = _index;
          if (ch == '-')
          {
            next();
          }
          while (Char.IsDigit(Current))
          {
            next();
          }
          if (Current == '.')
          {
            next();
            while (Char.IsDigit(Current))
            {
              next();
            }
          }
          else
          {
            string number = _text.Substring(start, _index - start);
            if (number.Length > 1 && number.StartsWith('0') || number.StartsWith("-0"))
            {
              _errors.Add($"{number} - Leading zeros are not allowed");
            }
          }

          _tokens.Add(new Token(TokenType.NUMBER, _text.Substring(start, _index - start)));
        }
        else if (ch == '"')
        {
          _tokens.Add(new Token(TokenType.QUOTE, "\""));
          string value = "";
          next();
          int start = _index;
          while (Current != '"' && Current != '\0')
          {
            if (ESCAPE_SEQUENCE.Contains(Current))
            {
              _errors.Add($"Unescaped characters in string");
              return;
            }
            if (Current == '\\')
            {
              next();
              switch (Current)
              {
                case 'n':
                  value += '\n';
                  break;
                case 't':
                  value += '\t';
                  break;
                case 'r':
                  value += '\r';
                  break;
                case 'b':
                  value += '\b';
                  break;
                case 'f':
                  value += '\f';
                  break;
                case '\\':
                  value += '\\';
                  break;
                case '"':
                  value += '"';
                  break;
                default:
                  _errors.Add($"Invalid escape sequence \\{Current}");
                  break;
              }
            }
            else
            {
              value += Current;
            }
            next();

          }
          _tokens.Add(new Token(TokenType.STRING, value));
          _tokens.Add(new Token(TokenType.QUOTE, "\""));
          next();
        }
        else
        {

          Token token = ch switch
          {
            ',' => new Token(TokenType.COMMA, ","),
            ':' => new Token(TokenType.COLON, ":"),
            '{' => new Token(TokenType.OPENING_CURLY_BRACE, "{"),
            '}' => new Token(TokenType.CLOSING_CURLY_BRACE, "}"),
            '[' => new Token(TokenType.OPENING_BRACKET, "["),
            ']' => new Token(TokenType.CLOSING_BRACKET, "]"),
            _ => new Token(TokenType.INVALID, ch.ToString())
          };
          if (token.Type == TokenType.INVALID)
          {
            _errors.Add($"Unexpected character {ch}");
          }
          _tokens.Add(token);
          next();
        }
      }
    }
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
