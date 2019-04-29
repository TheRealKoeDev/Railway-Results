# Railway-Results #

Provides Monads for error handling inspired by Railway-Oriented-Programming.

### Result ##
Is a plain Monad to detect if a operation was successful.

 ```csharp
    Result successImplicit = true;
    Result errorImplicit = false;
    
    Result success = Result.Success();
    Result error = Result.Error();    
```

### Result with Value ##
Is a Monad that contains a Value if the oparation was successful.

 ```csharp
    Result<string> successImplicit = "OK";
    
    Result<string> success = Result<string>.Success("OK");
    Result<string> error = Result<string>.Error();  
```

### Result with Error ##
Is a Monad that contains a Error if the oparation failed.

 ```csharp
    ResultWithError<string> errorImplicit = "Not OK";
    
    ResultWithError<string> success = ResultWithError<string>.Success();
    ResultWithError<string> error = ResultWithError<string>.Error("Not OK");  
```

### Result with Value or Error ##
Is a Monad that contains Either a Value if the oparation was successful or an Error if the operation was a failture.

 ```csharp
    Result<string, int> successImplicit = "OK";
    Result<string, int> errorImplicit = 404;
    
    Result<string, int> success = Result<string, int>.Success("OK");
    Result<string, int> error = Result<string, int>.Error(404);  
```

## Usage ##

* All Results have the Methods Do, OnSuccess, OnError, Either, Match, Bind, BindError, Keep, Ensure.
* If a Result has a Value and OnSuccess, Either, Match, Keep or Ensure is called the path that handles the Success requires the usage of the value. The same goes for the Error.
* All Methods throw an ArgumentNullException if a parameter of Action or Func is null.
* Exceptions are not handled by the Results.

### Bind ###
If the Result is a Success it turns into the outcome of the Func, otherwise it returns as Error of the new type.
The method can be overloaded to also specify wich Result it should be turned into in case of an Error.

 ```csharp
    Result.Success()
    .Bind(() => Result<int>.Success(200))
    .Bind(value => Result<string, string>.Success("OK"), () => Result<string, string>.Error("Error"));
```

### BindOnError ###
If the Result is a Error it turns into the outcome of the Func, otherwise it returns as Success of the new type.

 ```csharp
    Result.Success()
    .BindOnError(() => ResultWithError<int>.Error(500))
    .BindOnError(error => Result.Error());
```

### Do ###
Calls the Action in either case

```csharp
    Result.Success().Do(() => Console.WriteLine("Hi"));     
```

### OnSuccess ###
Calls the Action or Func if the Result is a Success.
It transforms the Value of the Result if a Func is given and turns the Result into a type that can hold the Value if needed.

 ```csharp
    Result.Success()
    .OnSuccess(() => Console.WriteLine("OK"))
    .OnSuccss(() => "OK")     // Result<string>
    .OnSuccess(value => 100); // Result<int>      
```

### OnError ###
Calls the Action or Func if the Result is a Error.
It transforms the Error of the Result if a Func is given and turns the Result into a type that can hold the Error if needed.

 ```csharp
    Result.Error()
    .OnError(() => Console.WriteLine("Not OK"))
    .OnError(() => "Not OK")  // ResultWithError<string>
    .OnError(error => 404);   // ResultWithError<int>      
```

### Either ###
Calls the left Action or Func if the Result is a success, otherwise calls the right one.

 ```csharp
    Result.Success()
    .Either(() =>  Console.WriteLine("Ok"), () => Console.WriteLine("Not OK"))
    .Either(() => "OK", () => Console.WriteLine("Help"))  // Result<string>
    .Either(value => 100, () => "rip")                    // Result<int, string>    
    .Either(value => value.ToString(), error => 500);     // Result<string, int>
```

### Match ###
Calls the left Func if the Result is a success, otherwise calls the right one.

 ```csharp
    Result.Success()
    .Match(() => 100, () => 404); // int
```

### Ensure ###
Turns the Result into a failture if it was a Success and the condition is false.

 ```csharp
    Result<DateTime>.Success(DateTime.Now)
    .Ensure(date => date > new DateTime(2020, 1, 1) && date < new DateTime(2021, 1, 1));
```

### Keep ###
Is a neat little feature that allows you to handle function calls that require multiple parameters,
but violates the best practice not to know the outcome of the Results before the call of Match.

 ```csharp
 
    public static void ShowText(string text)
    {
         Console.WriteLine(text);
         Thread.Sleep(2000);
    }
    
    public static void ShowText(TimeSpan duration, string text)
    {
         Console.WriteLine("The message was displayed for {duration}:");
         Console.WriteLine(text);
    }
    
    public static void Main()
    {
       Result.Success()
       .KeepEither(() => "Success", () => "Error", out string text)
       .Keep(() => DateTime.Now, out DateTime time)
       .Do(() => ShowText(text))
       .Keep(() => time.Subtract(DateTime.Now), out TimeSpan duration)
       .Do(() => ShowText(duration, text));
    }
```
