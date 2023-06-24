using Amazon.DynamoDBv2;
using Circuits.Public.DynamoDB;
using Circuits.Public.Http;
using Circuits.Public.UserInfo;
using Circuits.Public.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add AWS Lambda support. When application is run in Lambda Kestrel is swapped out as the web server with Amazon.Lambda.AspNetCoreServer. This
// package will act as the webserver translating request and responses between the Lambda event source and ASP.NET Core.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

builder.Services.AddTransient<CircuitsRepository>();
builder.Services.AddScoped<IDynamoDbContextWrapper, DynamoDbContextWrapper>();
builder.Services.AddScoped<AmazonDynamoDBClient>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IHttpClientWrapper, HttpClientWrapper>();
builder.Services.AddTransient<IEnvironmentVariableGetter, EnvironmentVariableGetter>();
builder.Services.AddTransient<IUserInfoGetter, UserInfoGetter>();
// Swagger: https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-7.0&tabs=visual-studio
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "Welcome to running ASP.NET Core Minimal API on AWS Lambda");

app.Run();
