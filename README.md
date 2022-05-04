# JSON-Parser

A simple JSON parser

# About

A toy library to parse JSON into C# objects([Dictionary](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2?view=net-6.0) and [List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1?view=net-6.0))

Do note that this project doesn't aim to implement the entire JSON Specification(which can be found [here](https://www.ietf.org/rfc/rfc4627.txt)), although it does conform to most of it. 

# Examples

## A Basic One

![image](https://user-images.githubusercontent.com/10794178/166743715-e7d3b890-8a04-4d5c-9230-53b340f553aa.png)

## One with more nesting

![image](https://user-images.githubusercontent.com/10794178/166744191-393afa36-aa86-49dc-a292-e1cea0f087d8.png)

## One without a closing bracket

![image](https://user-images.githubusercontent.com/10794178/166744310-bd882527-8113-4254-8c3f-bdac9759b8dc.png)

## One with a missing quote

![image](https://user-images.githubusercontent.com/10794178/166744447-4b8b5b37-34db-47cc-9db1-1427973c5141.png)

## One with single line comments

![image](https://user-images.githubusercontent.com/10794178/166823243-abb1d5ba-fb76-4f7f-8ba5-556d7b640b2d.png)

## One with multi line comments

![image](https://user-images.githubusercontent.com/10794178/166823416-ea57ef15-ed21-499a-9da5-358efe3ee936.png)


# Run

You can use the dotnet CLI to run the program. You need to pass the path of the file as an argument. 

```
dotnet run -- test.json
```
