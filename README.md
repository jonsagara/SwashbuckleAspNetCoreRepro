# Swashbuckle.AspNetCore 6.3.0 (and above) bug reproduction

After upgrading to `Swashbuckle.AspNetCore 6.3.0`, I started to get errors in my swagger docs whenever I clicked on an action name to expand the details:

![Swagger Error](https://github.com/jonsagara/SwashbuckleAspNetCoreRepro/blob/main/swagger_error.png?raw=true)

The error only seems to happen if the action captures route values using properties in a nested class.

This is a minimal reproduction using an ASP.NET Core MVC application with a test API controller named `TestController`.

**2022-07-19**: Verified that the bug still exists in `Swashbuckle.AspNetCore 6.4.0`.

## To view the working version ##

1. Check out branch `6.2.3`
1. Open the solution in Visual Studio 2022
1. Run the application
1. Navigate to `https://localhost:7211/swagger/` in your browser
1. Click on `/api/v1/Test/GetNonNested/{id}`; no error is thrown
1. Click on `/api/v1/Test/GetNested/{id}`; no error is thrown

## To view the runtime error ##

1. Check out branch `main`
1. Open the solution in Visual Studio 2022
1. Run the application
1. Navigate to `https://localhost:7211/swagger/` in your browser
1. Click on `/api/v1/Test/GetNonNested/{id}`; no error is thrown
1. Click on `/api/v1/Test/GetNested/{id}`; The error shown in the screenshot above is shown.

The action `TestController.GetNested(...)` uses an `int` property in a nested class to capture route values.

## Environment ##

- Visual Studio 2022 17.2.6
- .NET SDK 6.0.302
- Windows 10 Version 21H2 (OS Build 19044.1826)
