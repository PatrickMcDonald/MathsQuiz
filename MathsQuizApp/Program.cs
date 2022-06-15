using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

ConfigureMiddleware(app, app.Environment);
ConfigureEndpoints(app);

app.Run();

static void ConfigureServices(IServiceCollection services, ConfigurationManager configuration)
{
    services.Configure<CookiePolicyOptions>(options =>
    {
        // This lambda determines whether user consent for non-essential cookies is needed for a given request.
        options.CheckConsentNeeded = _ => true;
        options.MinimumSameSitePolicy = SameSiteMode.None;
        // Handling SameSite cookie according to https://docs.microsoft.com/en-us/aspnet/core/security/samesite?view=aspnetcore-3.1
        options.HandleSameSiteCookieCompatibility();
    });

    var initialScopes = configuration.GetValue<string>("DownstreamApi:Scopes")?.Split(' ');

    services.AddMicrosoftIdentityWebAppAuthentication(configuration)
        .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
        .AddMicrosoftGraph(configuration.GetSection("MicrosoftGraph"))
        .AddDownstreamWebApi("MathsQuizApi", configuration.GetSection("MathsQuizApi"))
        .AddInMemoryTokenCaches();

    services.AddAuthorization(options =>
    {
        // By default, all incoming requests will be authorized according to the default policy.
        options.FallbackPolicy = options.DefaultPolicy;
    });

    services.AddControllersWithViews(options =>
    {
        var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
        options.Filters.Add(new AuthorizeFilter(policy));
    });

    services.AddRazorPages()
        .AddMicrosoftIdentityUI();
}

static void ConfigureEndpoints(IEndpointRouteBuilder app)
{
    app.MapDefaultControllerRoute();
    app.MapRazorPages();
    app.MapControllers();
}

static void ConfigureMiddleware(IApplicationBuilder app, IWebHostEnvironment env)
{
    // Configure the HTTP request pipeline.
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();
}
