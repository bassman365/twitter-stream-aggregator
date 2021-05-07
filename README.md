# twitter-stream-aggregator

To run:

set Twitter API Token in appsettings.json:
```"Twitter": {
    "BaseUrl": "https://api.twitter.com",
    "Token": "{your-token-here}"
  }
```
or add a appsettings.Local.json file with a section with this override:
```
  "Twitter": {
    "Token": "{your-token-here}"
  }
```
This API is built with 
.NET 5.0. 

If the following shows a lack of a 5.0 version:
```
dotnet --version
```
Please ensure you install the appropriate runtime from here:
https://dotnet.microsoft.com/download/dotnet/5.0

Run in Visual Studio to use Swagger to exercise the API or

Run
```
dotnet run
```
from this directory:

{your-local-path}\twitter-stream-aggregator\src\TwitterStreamApi

and browse to:

https://localhost:5001/TwitterStreamStats

to observe the json.