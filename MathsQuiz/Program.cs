using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

ConfigureConfiguration(builder.Configuration);
ConfigureServices(builder.Services);

var app = builder.Build();

ConfigureMiddleware(app, app.Environment);
ConfigureEndpoints(app);

app.Run();

void ConfigureConfiguration(ConfigurationManager configuration) { }

void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();

    services.AddSwaggerGen(options =>
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
}

void ConfigureMiddleware(IApplicationBuilder app, IWebHostEnvironment env)
{
    // Configure the HTTP request pipeline.
    if (env.IsDevelopment() || env.IsStaging())
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

}

void ConfigureEndpoints(IEndpointRouteBuilder app)
{
    app.MapGet("/", (HttpResponse response) => response.RedirectTemporaryAsync("./swagger/index.html"));

    app.MapControllers();
}
