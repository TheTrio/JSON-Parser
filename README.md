# JSON-Parser

A simple JSON parser

# About

A toy library to parse JSON into C# objects([Dictionary](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2?view=net-6.0) and [List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1?view=net-6.0))

Do note that this project doesn't aim to implement the entire JSON Specification(which can be found [here](https://www.ietf.org/rfc/rfc4627.txt)), although it does conform to most of it. 

# Run

You can use the dotnet CLI to run the program. The program expects the json to be in `test.json`, which must be in the directory from where you run the program. 

```
dotnet run
```

# TODO

- [ ] Implement a CLI, which supports reading the json both from the standard input and a file.
- [ ] Add colors to the output
- [ ] Better error messages
- [ ] Add support for comments
