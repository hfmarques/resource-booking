using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Modules.Common.API;
using Modules.ResourceBooking.Core;
using Modules.ResourceBooking.Infrastructure.Persistence;
using Scalar.AspNetCore;
using Serilog;
using System.Text;
using ResourceBooking.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig.ReadFrom.Configuration(context.Configuration);
});

builder.AddServiceDefaults();

var jwtSecret = builder.Configuration["JwtSettings:Secret"] ?? "dummy_secret_key_that_is_long_enough_for_hmac_sha256";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "ResourceBookingIssuer",
            ValidAudience = "ResourceBookingAudience",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddOpenApi();

builder.AddNpgsqlDbContext<ResourceBookingDbContext>("resourcebooking");
builder.Services.AddResourceBookingModule(builder.Configuration);

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapOpenApi();
app.MapScalarApiReference();
if (app.Environment.IsDevelopment())
{
    app.Map("/", () => Results.Redirect("/scalar"));
}

app.UseSerilogRequestLogging();
app.UseAuthentication();
app.UseAuthorization();

app.MapApiEndpoints();

app.Run();
