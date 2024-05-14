# Swashbuckle.AspNetCore 6.3.0 (and above) bug reproduction

> **Update 2024-05-14:** There is a [workaround](https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/2703) documented in the issue tracker:
> 
> ```csharp
> builder.Services.AddSwaggerGen(c =>
> {
>     c.CustomSchemaIds(type => type.FullName.Replace("+", "."));
> });
> ```
> 
> This allows the nested types to function correctly in the generated swagger page.
> 
> Also, the project has [a new core team](https://github.com/domaindrivendev/Swashbuckle.AspNetCore/discussions/2778), and just [released an updated version](https://github.com/domaindrivendev/Swashbuckle.AspNetCore/releases/tag/v6.6.1).


After upgrading to `Swashbuckle.AspNetCore 6.3.0`, I started to get errors in my swagger docs whenever I clicked on an action name to expand the details:

![Swagger Error](https://github.com/jonsagara/SwashbuckleAspNetCoreRepro/blob/main/swagger_error.png?raw=true)

The error only seems to happen if the action captures route values using properties in a nested class.

This is a minimal reproduction using an ASP.NET Core MVC application with a test API controller named `TestController`.

**2023-01-11**: Verified that the bug still exists in `Swashbuckle.AspNetCore 6.5.0`.

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

- Visual Studio 2022 17.5.0 Preview 2.0
- .NET SDK 7.0.102
- Windows 10 Version 22H2 (OS Build 22621.1105)
