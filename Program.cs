using System;


namespace JSONParser
{
  class Start
  {

    public static void Main(string[] args)
    {
      var lexer = new Lexer("test.json");
      var parser = new Parser(lexer);
      try
      {
        var output = parser.ParseJson();
        Parser.PrettyPrint(output);
        Console.Write("\n");
      }
      catch (Exception err)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        foreach (var error in parser.Errors)
        {
          Console.WriteLine(error);
        }
        Console.WriteLine(err.Message);
        Console.ResetColor();
      }
    }
  }
}