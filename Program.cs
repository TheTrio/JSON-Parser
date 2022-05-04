using System;


namespace JSONParser
{
  class Start
  {

    public static void Main(string[] args)
    {
      var lexer = new Lexer("test.json");
      var parser = new Parser(lexer.Tokens);
      var output = parser.ParseJson();
      Console.WriteLine(Parser.PrettyPrint(output));
    }
  }
}