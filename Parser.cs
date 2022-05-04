using System;
using System.Collections.Generic;
using System.Linq;

namespace JSONParser
{
  public class Parser
  {
    private List<Token> _tokens;
    private int index;

    public List<Token> Tokens { get => _tokens; set => _tokens = value; }
    private List<string> _errors;
    public List<string> Errors => _errors;
    public Parser(Lexer lexer)
    {
      _tokens = lexer.Tokens;
      _errors = lexer.Errors;
      index = 0;
    }
    private Token next()
    {
      if (index >= _tokens.Count)
      {
        return new Token(TokenType.EOF, "\0");
      }
      return _tokens[index++];
    }

    private Token peek(int a = 1)
    {
      if (index + a >= _tokens.Count)
      {
        return new Token(TokenType.EOF, "\0");
      }
      return _tokens[index + a];
    }

    private Token Current => peek(0);

    private Token match(params TokenType[] types)
    {
      foreach (TokenType type in types)
      {
        if (Current.Type == type)
        {
          return next();
        }
      }
      throw new Exception($"Unexpected Token {Current.Type} '{Current.Value}'. Expected {types[0]}");
    }

    private Dictionary<string, dynamic> parse_object()
    {
      var dict = new Dictionary<string, dynamic>();
      match(TokenType.OPENING_CURLY_BRACE);
      while (Current.Type != TokenType.CLOSING_CURLY_BRACE)
      {
        var string_val = parse_string();
        match(TokenType.COLON);
        dict.Add(string_val, ParseJson());
        if (Current.Type == TokenType.COMMA)
        {
          next();
        }
      }
      match(TokenType.CLOSING_CURLY_BRACE);
      return dict;
    }

    private List<dynamic> parse_list()
    {
      var list = new List<dynamic>();
      match(TokenType.OPENING_BRACKET);
      while (Current.Type != TokenType.CLOSING_BRACKET)
      {
        list.Add(ParseJson());
        if (Current.Type == TokenType.COMMA)
        {
          next();
        }
        else
        {
          break;
        }
      }
      match(TokenType.CLOSING_BRACKET);
      return list;
    }
    private string parse_string()
    {
      match(TokenType.QUOTE);
      var stringToken = match(TokenType.STRING, TokenType.NUMBER);
      match(TokenType.QUOTE);
      return stringToken.Value;
    }
    public dynamic ParseJson()
    {
      if (_errors.Count > 0)
      {
        throw new Exception("Invalid JSON");
      }
      switch (Current.Type)
      {
        case TokenType.OPENING_CURLY_BRACE:
          return parse_object();
        case TokenType.NUMBER:
          return int.Parse(next().Value);
        case TokenType.QUOTE:
          return parse_string();
        case TokenType.OPENING_BRACKET:
          return parse_list();
        default:
          throw new Exception("Expected list, object, integer or string after \":\" Found nothing");
      };
    }
    public static void PrettyPrint(dynamic arg, int indent = 1)
    {
      if (arg is int)
      {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(arg);
        Console.ResetColor();
        return;
      }
      if (arg is string)
      {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"\"{arg}\"");
        Console.ResetColor();
        return;
      }
      var padding = new string(' ', indent * 2);
      if (arg is List<dynamic> list)
      {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("[");
        Console.ResetColor();
        for (int i = 0; i < list.Count; i++)
        {
          PrettyPrint(list[i]);
          if (i != list.Count - 1)
            Console.Write(", ");
        }
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("]");
        Console.ResetColor();
      }
      if (arg is Dictionary<string, dynamic> dict)
      {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"{{\n");
        Console.ResetColor();
        foreach (var pair in dict)
        {
          // Console.Write($"{padding}{pair.Key} -> ");
          Console.ForegroundColor = ConsoleColor.Magenta;
          Console.Write($"{padding}{pair.Key}");
          Console.ResetColor();
          Console.Write(" -> ");
          PrettyPrint(pair.Value, indent + 1);
          Console.Write("\n");
        }
        padding = new string(' ', (indent - 1) * 2);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"{padding}}}");
        Console.ResetColor();
      }
      Console.Write("");
    }
  }
}