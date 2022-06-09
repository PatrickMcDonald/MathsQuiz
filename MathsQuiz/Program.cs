using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "Maths Quiz API",
            Version = "0.1"
        });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    options.EnableAnnotations();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger(options =>
    {
        options.PreSerializeFilters.Add((swagger, httpReq) =>
        {
            swagger.Servers = new[]
            {
                new OpenApiServer
                {
                    Url = $"{httpReq.Scheme}://{httpReq.Host.Value}",
                    Description = "host",
                },
            };
        });
    });

    app.UseSwaggerUI(options =>
    {
        options.DocumentTitle = "Maths Quiz API - Swagger UI";
        options.RoutePrefix = "swagger";
        options.SwaggerEndpoint("v1/swagger.json", "Maths Quiz API V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGet("/", request => request.Response.RedirectTemporaryAsync("./swagger/index.html"));

app.MapControllers();

app.Run();
