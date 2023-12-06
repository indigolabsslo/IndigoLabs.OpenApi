using IndigoLabs.OpenApi.Minimal.Endpoints;
using IndigoLabs.OpenApi.Shared;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
  options.SwaggerDoc("v1", new OpenApiInfo
  {
    Version = "v1",
    Title = "OpenApi Demo - Controller",
    Description = "A web api showcasing how to create a good open api specification in minimal api project.",
    TermsOfService = new Uri("https://indigo.si"),
    Contact = new OpenApiContact
    {
      Name = "Luka Zlatecan",
      Url = new Uri("https://indigo.si/contact/")
    },
  });

  // Just for executing project
  //var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
  //options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

  //For all projects. Can be a problem if other .xml files are present
  List<string> xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly).ToList();
  xmlFiles.ForEach(xmlFile => options.IncludeXmlComments(xmlFile));
});

builder.Services.AddSingleton<CarRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

new CarMinimalFluent(app.Services.GetService<CarRepository>()!).DefineEndpoints(app);
new CarMinimalOperation(app.Services.GetService<CarRepository>()!).DefineEndpoints(app);
new CarMinimalXml(app.Services.GetService<CarRepository>()!).DefineEndpoints(app);

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
  public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
