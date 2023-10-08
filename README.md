# Fakes.Azure.Functions

## Introduction
A class library to assist with creating unit tests for HTTP Triggers in Azure Function Apps using the isolated process model.

## Usage
It is quite simple to get started using the Fakes for writing unit tests.
Let's say you have a HTTP Trigger function

``` C#
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace MyFunctionApp;

public class MyFunction
{
    private readonly ILogger<MyFunction> _logger;

    public MyFunction(ILogger<MyFunction> logger)
    {
        _logger = logger;
    }

    [Function("MyFunction")]
    public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData request)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        var response = request.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

        response.WriteString("Welcome to Azure Functions!");

        return response;
    }
}
```

You wish to write a unit test to verify the function returns a HTTP status code OK.

``` c#
using Fakes.Azure.Functions;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using MyFunctionApp;
using Moq;
using System.Net;
using Xunit;

namespace MyFunctionApp.Tests;

public class MyFunctionTests
{
    [Fact]
    public void Function_Returns_Ok()
    {
        Mock<ILogger<MyFunction>> logger = new();

        MyFunction functionUnderTest = new(logger.Object);
        FakeHttpRequestData request = new("https://www.contoso.com");
        HttpResponseData response = functionUnderTest.Run(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
```

If the function to test has a request body, you have two choices:
- Instantiate a ```FakeHttpRequest ``` object using the constructor overload ```FakeHttpRequest(string uri, string body)```
- Instantiate a ```FakeHttpRequest``` object using the static method ```FakeHttpRequest.CreateInstance<TBody>(string uri, TBody body)```

In either case the resulting ```HttpRequestData``` instance will have a ```Body``` stream populated with the provided content.

## Features
The provided functionality is provided as a starting point and will likely cover most situations. However, advanced usage of HTTP triggers may require implementing more properties in ```FakeHttpRequestData```, ```FakeFunctionContext``` or ```FakeHttpResponseData```.

Currently the features supported include:
* Request bodies (POST, PUT)
* Cancellation Tokens
* Response bodies
* Headers (Request & Response)

In cases where a feature is not supported, a ```NotImplementedException``` will be thrown. Contributing to the Fake implementations is highly encouraged.
