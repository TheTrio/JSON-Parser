using System;
using System.Collections.Generic;
using System.Linq;

namespace JSONParser
{
  public class Parser
  {
    private List<Token> tokens;
    private int index;

    public List<Token> Tokens { get => tokens; set => tokens = value; }
    public Parser(List<Token> t)
    {
      tokens = t;
      index = 0;
    }
    private Token next()
    {
      if (index >= tokens.Count)
      {
        return new Token(TokenType.EOF);
      }
      return tokens[index++];
    }

    private Token peek(int a = 1)
    {
      if (index + a >= tokens.Count)
      {
        return new Token(TokenType.EOF);
      }
      return tokens[index + a];
    }

    private Token Current => peek(0);

    private Token match(TokenType type)
    {
      if (Current.Type != type)
      {
        throw new Exception($"Expected {type}, found {Current.Type}");
      }
      return next();
    }

    private Dictionary<string, dynamic> parse_object()
    {
      var dict = new Dictionary<string, dynamic>();
      match(TokenType.OPENING_CURLY_BRACE);
      while (Current.Type != TokenType.CLOSING_CURLY_BRACE)
      {
        var stringToken = match(TokenType.STRING);
        match(TokenType.COLON);
        dict.Add(stringToken.Value, ParseJson());
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
    public dynamic ParseJson()
    {
      switch (Current.Type)
      {
        case TokenType.OPENING_CURLY_BRACE:
          return parse_object();
        case TokenType.NUMBER:
          return int.Parse(next().Value);
        case TokenType.STRING:
          return next().Value;
        case TokenType.OPENING_BRACKET:
          return parse_list();
        default:
          throw new Exception("Expected value, found nothing");
      };
    }
    public static string PrettyPrint(dynamic arg, int indent = 1)
    {
      if (arg is int)
      {
        return $"{arg}";
      }
      if (arg is string)
      {
        return arg;
      }
      var padding = new string(' ', indent * 2);
      if (arg is List<dynamic> list)
      {
        return $"[{string.Join(", ", list.Select(item => PrettyPrint(item)))}]";
      }
      string output = $"{{\n";
      if (arg is Dictionary<string, dynamic> dict)
      {
        foreach (var pair in dict)
        {
          output += $"{padding}{pair.Key} -> {PrettyPrint(pair.Value, indent + 1)}\n";
        }
        padding = new string(' ', (indent - 1) * 2);
        output += $"{padding}}}";
        return output;
      }
      return "";
    }
  }
}