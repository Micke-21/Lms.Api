using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Lms.Data.Data;
using Lms.Api.Extensions;
using Newtonsoft.Json.Serialization;
using Lms.Data.Mappers;
using Lms.Data.DAL;
using Lms.Core.IDAL;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LmsApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LmsApiContext")));

// Add services to the container.

builder.Services.AddControllers(opt=>opt.ReturnHttpNotAcceptable = true)
    .AddNewtonsoftJson()
    .AddNewtonsoftJson(setupAction =>
    {
        setupAction.SerializerSettings.ContractResolver =
            new CamelCasePropertyNamesContractResolver();
    })
    .AddXmlDataContractSerializerFormatters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setupAction =>
{
    var xmlCommentsFils = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFils);

    setupAction.IncludeXmlComments(xmlCommentsFullPath);
});

builder.Services.AddAutoMapper(typeof(LmsMappings));


IServiceCollection serviceCollection = builder.Services.AddScoped<ICourseLibraryRepository, CourseLibraryRepository>();


var app = builder.Build();
app.SeedDataAsync().GetAwaiter().GetResult();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
