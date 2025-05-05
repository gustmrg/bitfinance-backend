using BitFinance.API.Extensions;
using BitFinance.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.AddSerilogLogging();

builder.Services.AddCorsPolicy();
builder.Services.AddProxyForwardingSupport();
builder.Services.AddJwtConfiguration(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddPersistence();
builder.Services.AddCaching(builder.Configuration);
builder.Services.AddApiConfiguration();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddApiVersioningSupport();
builder.Services.AddHttpRequestLogging();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseSwaggerDocumentation();
app.UseCorsPolicy();
app.UseForwardedHeaders();

app.UseAuthentication();
app.UseAuthorization();
app.UseOrganizationAuthorization();

if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
}

if (app.Environment.IsProduction())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();