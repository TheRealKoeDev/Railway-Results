# Railway-Results #

Provides Monads for error handling inspired by Railway-Oriented-Programming.

This Library allows you to handle functions that might fail or produce Errors easily by providing two paths for return values
and allows you to place error creation inside of functions without the use of Exceptions.
Errors are passed on automatically and can be handled later without the need to box functions.

NameSpace: KoeLib.Patterns.Railway

* Monads/Results: KoeLib.Patterns.Railway.Results
* Task Extensions for Monads: KoeLib.Patterns.Railway.Tasks
* IEnumerable Extensions for Monads: KoeLib.Patterns.Railway.Linq

#### NuGet
````
Install-Package Railway-Results
````

## Monads
* [Result](https://github.com/TheRealKoeDev/Railway-Results/wiki/Result)
* [ResultᐸValueᐳ](https://github.com/TheRealKoeDev/Railway-Results/wiki/ResultᐸValueᐳ)
* [ResultOrErrorᐸErrorᐳ](https://github.com/TheRealKoeDev/Railway-Results/wiki/ResultOrErrorᐸErrorᐳ)
* [ResultᐸValue, Errorᐳ](https://github.com/TheRealKoeDev/Railway-Results/wiki/ResultᐸValue,-Errorᐳ)

### Example: ResultᐸValue, Errorᐳ
```csharp
public class Error
{
    public string Message { get; }
    public DateTimeOffset CreatedAt { get; }
    public Exception Exception { get; }

    public Error(string message, Exception ex = null)
    {
        Message = message;
        CreatedAt = DateTimeOffset.Now;
        Exception = ex;
    }

    public override string ToString()
    {
        return  $"Error: {Message} \n" +
                $"Time: {CreatedAt} \n" +                       
                (Exception == null ? string.Empty : "Exception: " + Exception.Message);
    }
}

public static Result<string, Error> GetPageContent(string address)
{
    try
    {
        using (WebClient client = new WebClient())
        {
            return client.DownloadString(address);
        }               
    }
    catch(Exception e)
    {
        return new Error("Download failed.", e);
    }
}

public static Result<string, Error> StoreContent(string content, string filename)
{
    try
    {
        DirectoryInfo directory = Directory.CreateDirectory(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "example-downloads")));
        File.WriteAllText(directory.FullName + "\\" + Path.GetFileName(filename), content);
        return "Message: Content was sucessfully stored.";
    }
    catch(Exception e)
    {
        return new Error("Storage failed.", e);
    }
}

public static void Main()
{
    string address = "https://www.google.com";

    string message= GetPageContent(address)
    .Bind(content => StoreContent(content, address + ".html"))
    .Match(msg => msg , error => error.ToString());

    Console.WriteLine($"{address} \n{message}");
    Console.ReadLine();
}
```
