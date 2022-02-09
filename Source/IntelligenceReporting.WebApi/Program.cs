using IntelligenceReporting.Databases;
using IntelligenceReporting.Repositories;
using IntelligenceReporting.Settings;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
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

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
