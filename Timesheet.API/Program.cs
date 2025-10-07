using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;
using Timesheet.API.DbContexts;
using Timesheet.API.Repositories;
using Timesheet.API.Repositories.IRepositories;
using Timesheet.API.Services;
using Timesheet.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IUserAccountService, UserAccountService>();
builder.Services.AddScoped<ITimeEntryService, TimeEntryService>();

builder.Services.AddScoped<IUserAccountRepository, UserAccountRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ITimeEntryRepository, TimeEntryRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

if (builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddDbContext<TimesheetContext>(options =>
        options.UseInMemoryDatabase("TimesheetTestDb"));
}
else
{
    builder.Services.AddDbContext<TimesheetContext>(
    //=> dbContextOptions.UseSqlite(
    //    builder.Configuration["ConnectionStrings:TimesheetDbConnectionString"]));
    dbContextOptions => dbContextOptions.UseSqlServer(
        builder.Configuration["ConnectionStrings:TimesheetDbConnectionString"])
    .EnableSensitiveDataLogging());
}

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new ()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Authentication:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Convert.FromBase64String(builder.Configuration["Authentication:SecretForKey"]!))
        };
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddApiVersioning(setupAction =>
//{
//    setupAction.AssumeDefaultVersionWhenUnspecified = true;
//    setupAction.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
//    setupAction.ReportApiVersions = true;
//}).AddMvc();

// This exposes version groups to Swagger
//builder.Services.AddVersionedApiExplorer(o =>
//{
//    o.GroupNameFormat = "'v'VVV";               // v1, v1.1, v2
//    o.SubstituteApiVersionInUrl = true;
//});

// also register docs for each discovered version
builder.Services.AddSwaggerGen(options =>
{
    //using var scope = builder.Services.BuildServiceProvider().CreateScope();
    //var apiProvider = scope.ServiceProvider.GetRequiredService<IApiVersionDescriptionProvider>();

    //foreach (var desc in apiProvider.ApiVersionDescriptions)
    //{
        //options.SwaggerDoc(desc.GroupName, new OpenApiInfo
        //{
        //    Title = "Timesheet.API",
        //    Version = desc.ApiVersion.ToString(),
        //    Description = desc.IsDeprecated ? "This API version is deprecated." : null
        //});
    //}

    //var xmlCommentsFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
    //options.IncludeXmlComments(xmlCommentsFullPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        //var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        //foreach (var desc in provider.ApiVersionDescriptions)
        //{
        //    c.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json",
        //                      $"Timesheet.API {desc.GroupName.ToUpper()}");
        //}
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
