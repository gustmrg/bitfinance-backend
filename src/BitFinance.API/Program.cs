using BitFinance.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpOptions();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddDatabaseContext(builder.Configuration);
builder.Services.AddDependencyInjection();
builder.Services.AddCaching(builder.Configuration);
builder.Services.AddApiDocumentation();
builder.Services.AddHttpLogging();

builder.Host.AddLogging(builder.Configuration);

var app = builder.Build();

app.ConfigureMiddleware(builder.Configuration);

app.Run();