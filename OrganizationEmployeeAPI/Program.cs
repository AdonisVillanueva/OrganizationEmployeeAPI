using Microsoft.EntityFrameworkCore;
using OrganizationEmployeeAPI.Repository;
using Microsoft.Extensions.FileProviders;
using OrganizationEmployeeAPI.Contracts;
using OrganizationEmployeeAPI;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
}); //json options not default to camel case
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    //options.SwaggerDoc("v1", new OpenApiInfo
    //{
    //    Title = "Web API",
    //    Version = "v1"
    //});
    options.OperationFilter<FileUploadFilter>();
});

builder.Services.AddScoped<IDepartment, DepartmentRepository>();
builder.Services.AddScoped<IEmployee, EmployeeRepository>();
builder.Services.AddScoped<IFileService, FileRepository>();

ConfigurationManager Configuration = builder.Configuration;

IServiceCollection serviceCollection = builder.Services.AddDbContext<APIDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("OrganizationEmployeeCon")));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "orgEmpPolicy",
                policy =>
                {
                     policy.WithOrigins("http://localhost:3000", "http://192.168.1.9:3000","https://localhost:7224", "https://127.0.0.1:7224")
                    .WithHeaders(HeaderNames.ContentType)
                    .WithMethods("PUT", "DELETE", "GET", "PATCH","POST");
                });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseCors("orgEmpPolicy");

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "Photos")),
    RequestPath = "/Photos"
});

//app.UseAuthorization();

app.MapControllers();

app.Run();
