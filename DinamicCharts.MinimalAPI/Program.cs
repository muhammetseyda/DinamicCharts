using DinamicCharts.Application.DTOs.ConnectionInfo;
using DinamicCharts.Application.Helpers.TokenHelper;
using DinamicCharts.Application.Interfaces;
using DinamicCharts.Infrasturucture;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = "dinamicChart",
            ValidAudience = "dinamicChartUser",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();


builder.Services.AddScoped<IDbInspectorServices, DbInspectorServices>();
builder.Services.AddScoped<ITokenHelper, TokenHelper>();

var app = builder.Build();

app.MapScalarApiReference(
    opt =>
    {
        opt.Title = "Dinamic Charts";
        opt.Theme = ScalarTheme.BluePlanet;
        opt.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);

    });
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapPost("/api/token", (LoginDto dto, ITokenHelper token) =>
{
    var validUsers = new List<(string Username, string Password)>
    {
        ("seyda", "Aa123456."),
        ("admin", "Aa123456.")
    };

    bool isValid = validUsers.Any(u => u.Username == dto.Username && u.Password == dto.Password);

    if (!isValid)
    {
        return Results.Unauthorized();
    }
    var result = token.GenerateToken(dto.Username, dto.Password);
    return Results.Ok(new { token = result });
});

app.MapPost("/api/inspect/views", async (ConnectionInfoDto dto, IDbInspectorServices service) =>
{
    var result = await service.GetViewsAsync(dto);
    return Results.Ok(result);
}).RequireAuthorization();
app.MapPost("/api/inspect/procedures", async (ConnectionInfoDto dto, IDbInspectorServices service) =>
{
    var result = await service.GetStoredProceduresAsync(dto);
    return Results.Ok(result);
}).RequireAuthorization();

app.MapPost("/api/inspect/functions", async (ConnectionInfoDto dto, IDbInspectorServices service) =>
{
    var result = await service.GetFunctionsAsync(dto);
    return Results.Ok(result);
}).RequireAuthorization();

app.MapPost("/api/execute-object", async (ExecuteObjectRequest request, IDbInspectorServices service) =>
{
    var result = await service.ExecuteObjectAsync(request.ToConnectionInfoDto(), request.ObjectType, request.ObjectName);
    return Results.Ok(result);
}).RequireAuthorization();

app.Run();

class ExecuteObjectRequest
{
    public string Host { get; set; }
    public string DbAdi { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string ObjectType { get; set; }
    public string ObjectName { get; set; }
    public ConnectionInfoDto ToConnectionInfoDto()
    {
        return new ConnectionInfoDto
        {
            Host = Host,
            DbAdi = DbAdi,
            Username = Username,
            Password = Password
        };
    }
}
class LoginDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}