using System.IO.Compression;
using System.Text;
using IntelligenceReporting.Authentication;
using IntelligenceReporting.Databases;
using IntelligenceReporting.Repositories;
using IntelligenceReporting.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Amazon.Extensions.NETCore.Setup;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// The ASPNETCORE_ENVIRONMENT environment variable should be set to "Staging", otherwise it is assumed to be Production.
// In a local development environment the launchSettings.json file will override this and set the environment too Development.
// The ASPNETCORE_ENVIRONMENT environment variable can be set using this command line instruction "setx ASPNETCORE_ENVIRONMENT "Staging" /M"

// Add services to the container.
builder.Services.AddCors(policy =>
{
    IConfiguration configuration = builder.Configuration;   

    var allowedOrigins = configuration["AllowedHosts"].Split(",");
    policy.AddPolicy("CorsPolicy", opt => opt
        .WithOrigins(allowedOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .WithExposedHeaders("*"));
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<IntelligenceReportingDbContext>();

var environment = builder.Environment;
if (!environment.IsDevelopment())
{
    // The config when in staging or production is source from AWS parameter Source.
    // The IAM role that the EC2 instance runs under has permission to access the paramter store.
    // When we call AddSystemsManager below with no credentials the IAM role will be used to get the config.
    // The AWSSDK.SecurityToken Nuget package is used when deployed in order to assume roles.

    AWSOptions options = new()
    {
        Region = Amazon.RegionEndpoint.APSoutheast2
    };
    builder.Services.AddDefaultAWSOptions(options);
    builder.Configuration.AddSystemsManager($"/intelligence-reporting/{environment.EnvironmentName}/", options);
    builder.Services.AddDataProtection().PersistKeysToAWSSystemsManager($"/intelligence-reporting/{environment.EnvironmentName}/VaultDbSettings");
}

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services
    .AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
            ValidAudience = jwtSettings.GetSection("validAudience").Value,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetSection("securityKey").Value))
        };
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = $"Intelligence Reporting API - {environment.EnvironmentName}"
    });

    // Use XML doc for Swagger
    foreach (var xmlFile in Directory.GetFiles(AppContext.BaseDirectory, "IntelligenceReporting.*.xml"))
    {
        c.IncludeXmlComments(xmlFile);        
    }    
});

// Generically register all IntelligenceReporting.Services by their IntelligenceReporting.Interfaces
var interfacesAssembly = typeof(IRepository<>).Assembly;
foreach (var type in typeof(Repository<>).Assembly.GetTypes())
{
    if (type.IsAbstract) continue;

    foreach (var interfaceType in type.GetInterfaces())
    {
        if (interfaceType.Assembly != interfacesAssembly) continue;

        builder.Services.AddScoped(interfaceType, type);
    }
}

var intelligenceReportingConnectionString = builder.Configuration.GetConnectionString("IntelligenceReporting");
builder.Services.AddDbContext<IntelligenceReportingDbContext>(item => item.UseSqlServer(intelligenceReportingConnectionString));

builder.Services.AddScoped(_ =>
    {
        var vaultDbSettings = new VaultDbSettings();
        builder.Configuration.Bind("VaultDbSettings", vaultDbSettings);
        return vaultDbSettings;
    });
builder.Services.AddScoped<IVaultDatabase, VaultDatabase>();
builder.Services.AddScoped<IVaultDatabase2, VaultDatabase>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<BrotliCompressionProvider>();
    options.EnableForHttps = true;
});
builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Optimal;
});

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthentication(); 
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
