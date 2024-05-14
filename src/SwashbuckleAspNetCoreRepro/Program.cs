using System.Reflection;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using SwashbuckleAspNetCoreRepro.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services
    .AddApiVersioning(options =>
    {
        // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
        options.ReportApiVersions = true;
    })
    .AddMvc()
    .AddApiExplorer(options =>
    {
        // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
        // note: the specified format code will format the version as "'v'major[.minor][-status]"
        options.GroupNameFormat = "'v'VVV";

        // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
        // can also be used to control the format of the API version in route templates
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

builder.Services
    .AddSwaggerGen(
        options =>
        {
            // add a custom operation filter which sets default values
            options.OperationFilter<SwaggerDefaultValues>();

            // integrate xml comments
            options.IncludeXmlComments(GetXmlCommentsFilePath(builder));

            // Since our MediatR classes all end with either Query or Command, we have to ensure the swagger schema uses
            //   the full class names in order to avoid conflicts while generating swagger.json.
            // Use "." as separators instead of "+" so that nested types will work.
            //   See: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/2703#issuecomment-1707894923
            options.CustomSchemaIds(s => s.FullName?.Replace("+", ".", StringComparison.Ordinal));

            // Allow POST/PUT API endpoints to have private setters on List<T> and still render in the swagger docs.
            //   See: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/1536#issuecomment-607140245
            options.EnableAnnotations();
        });

// As of Swashbuckle 5.0.0, we have to explicitly opt-in to Newtonsoft.Json support.
builder.Services.AddSwaggerGenNewtonsoftSupport();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseSwagger();

var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseSwaggerUI(
    options =>
    {
        // build a swagger endpoint for each discovered API version
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());

            options.EnableDeepLinking();
        }
    });

app.Run();


static string GetXmlCommentsFilePath(WebApplicationBuilder builder)
{
    var basePath = builder.Environment.ContentRootPath;
    var fileName = typeof(Program).GetTypeInfo().Assembly.GetName().Name + ".xml";

    return Path.Combine(basePath, "Resources", fileName);
}