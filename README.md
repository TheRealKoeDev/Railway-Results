# Railway-Results #

Provides Monads for error handling inspired by Railway-Oriented-Programming.

NameSpace: KoeLib.Patterns.Railway

* Monads/Results: KoeLib.Patterns.Railway.Results
* Task Extensions for Monads: KoeLib.Patterns.Railway.Tasks
* IEnumerable Extensions for Monads: KoeLib.Patterns.Railway.Linq

## Monads
* [Result](/TheRealKoeDev/Railway-Results/wiki/Result)
* [ResultWithValue](/TheRealKoeDev/Railway-Results/wiki/ResultWithValue)
* [ResultWithError](/TheRealKoeDev/Railway-Results/wiki/ResultWithError)
* [ResultWithValueOrError](/TheRealKoeDev/Railway-Results/wiki/ResultWithValueOrError)


## Usage ##

* All Results have the methods Do, OnSuccess, OnError, Either, Match, Bind, BindError, Keep, Ensure.
* If a Result has a Value and OnSuccess, Either, Match, Keep or Ensure is called the path that handles the Success requires the usage of the value. The same goes for the Error.
* All Methods throw an ArgumentNullException if a parameter of Action or Func is null.
* Exceptions are not handled by the Results.
