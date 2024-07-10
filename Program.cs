var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

    // Log the incoming request details without the HTTP method
    logger.LogInformation("Received request: {Path} at {Time}",
                          context.Request.Path,
                          DateTime.UtcNow);

    // Process the next middleware in the pipeline
    await next.Invoke();

    // Get the endpoint details after the request is processed
    var endpoint = context.GetEndpoint();
    if (endpoint != null)
    {
        var routePattern = endpoint?.Metadata.GetMetadata<Microsoft.AspNetCore.Routing.RouteNameMetadata>()?.RouteName;
        var controllerActionDescriptor = endpoint.Metadata.GetMetadata<Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor>();

        //var controllerName = controllerActionDescriptor?.ControllerName ?? "Unknown Controller";
        //var actionName = controllerActionDescriptor?.ActionName ?? "Unknown Action";

        // Log the response details including the controller and action method names
        logger.LogInformation("Response status: {StatusCode} for {Path} at {Time}",
                              context.Response.StatusCode,
                              context.Request.Path,
                              DateTime.UtcNow);
    }
    else
    {
        // Log the response details if no endpoint metadata is available
        logger.LogInformation("Response status: {StatusCode} at {Time}",
                              context.Response.StatusCode,
                              DateTime.UtcNow);
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapReverseProxy();
app.UseRouting();
app.UseCors("corsapp");
app.UseAuthorization();

app.MapControllers();

app.Run();
