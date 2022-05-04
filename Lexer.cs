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
    QUOTE,
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
        char ch = current();
        if (ch == '\0')
        {
          _tokens.Add(new Token(TokenType.EOF, "\0"));
          break;
        }
        if (Char.IsWhiteSpace(ch))
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
          _tokens.Add(new Token(TokenType.STRING, _text.Substring(start, _index - start)));
        }
        else if (Char.IsDigit(ch))
        {
          int start = _index;
          while (Char.IsDigit(Current))
          {
            next();
          }
          _tokens.Add(new Token(TokenType.NUMBER, _text.Substring(start, _index - start)));
        }
        else if (ch == '"')
        {
          _tokens.Add(new Token(TokenType.QUOTE, "\""));
          next();
          int start = _index;
          while (Current != '"' && Current != '\0')
          {
            next();
          }
          _tokens.Add(new Token(TokenType.STRING, _text.Substring(start, _index - start)));
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