﻿using Microsoft.AspNetCore.Mvc;

namespace SwashbuckleAspNetCoreRepro.Controllers.Api.V1;

/// <summary>
/// Test API controller.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class TestController : ControllerBase
{
    /// <summary>
    /// This is a test method with a nested class to capture parameters.
    /// </summary>
    /// <returns>An object with a message.</returns>
    [HttpGet("GetNested/{id}")]
    public async Task<IActionResult> GetNested(NestedActionModel.Query query)
    {
        await Task.CompletedTask;

        return Ok(new { message = "this is a nested class test" });
    }

    /// <summary>
    /// This is a test method with a nested class to capture parameters.
    /// </summary>
    /// <returns>An object with a message.</returns>
    [HttpGet("GetNonNested/{id}")]
    public async Task<IActionResult> GetNonNested(int id)
    {
        await Task.CompletedTask;

        return Ok(new { message = "this is a non-nested class test" });
    }
}

/// <summary>
/// XML documentation for top-level NestedActionModel class.
/// </summary>
public class NestedActionModel
{
    /// <summary>
    /// XML documentation for nested NestedActionModel.Query class.
    /// </summary>
    public class Query
    {
        /// <summary>
        /// Id property used in the getter.
        /// </summary>
        public int Id { get; set; }
    }
}
