using Daemo.SDK;
using DaemoNETDemo.Client.Pages;
using DaemoNETDemo.Components;
using DaemoNETDemo.Components.Account;
using DaemoNETDemo.Data;
using DaemoNETDemo.Functions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MudBlazor;
using MudBlazor.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddControllers(); // 1. Add this line to register Controller services


builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();
builder.Services.AddMudServices();
builder.Services.AddMudMarkdownServices();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false) // Do not require the user to have a confirmed email account to sign in.
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddSignInManager()
//    .AddDefaultTokenProviders();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // Allow sign-in without email confirmation
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager<SignInManager<ApplicationUser>>()
    .AddDefaultTokenProviders();

// In the Server's Program.cs
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.Lax; // Allows cookies on top-level navigations
    options.Cookie.SecurePolicy = CookieSecurePolicy.None; // Requires HTTPS
    options.Cookie.HttpOnly = true;
});

builder.Services.AddScoped(sp =>
{
    var navigationManager = sp.GetRequiredService<NavigationManager>();
    return new HttpClient
    {
        BaseAddress = new Uri(navigationManager.BaseUri)
    };
});
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddScoped<DaemoAIService>();


// --- START: CONFIGURATION LOADING ---
var daemoConfig = builder.Configuration.GetSection("Daemo");
var llmConfig = builder.Configuration.GetSection("Llm");

// --- END: CONFIGURATION LOADING ---

// --- Configure Daemo PRODUCER SDK (Hosted Connection) ---
builder.Services.AddDaemoHosted(daemo =>
{
    // This action is deferred until the service provider is built, avoiding the ASP0000 warning.
    // We register the functions that this specific microservice will provide to the agent.

    var serviceProvider = builder.Services.BuildServiceProvider();
    var DeamoFunctions = serviceProvider.GetRequiredService<DaemoAIService>();

    daemo.WithServiceName("daemodemoservice")
        .WithSystemPrompt(@"
            You are an intelligent database assistant helping Administration and Employees retrieve meaningful data using prompts
        ")
        .RegisterService(DeamoFunctions);
},
options => {
    // This configures the connection details for the background service.
    options.DaemoGatewayUrl = daemoConfig["GatewayUrl"]!;
    options.AgentApiKey = daemoConfig["AgentApiKey"];
});

// --- Configure Daemo CONSUMER SDK (Agent Client) ---
// 1. Configure the options for the DaemoClient
builder.Services.Configure<DaemoAgentOptions>(options =>
{
    options.AgentEndpoint = daemoConfig["AgentEndpoint"]!;
    options.AgentApiKey = daemoConfig["AgentApiKey"];
    options.DefaultLlmConfig = new DaemoLlmConfig
    {
        Provider = llmConfig["Provider"] ?? "gemini",
        Model = llmConfig["Model"]
    };
});

// 2. Register DaemoClient as a singleton.
// The DI container will automatically find the IOptions<DaemoAgentOptions>,
// ILogger<DaemoClient>, and the singleton GrpcChannelManager and pass them to the constructor.

builder.Services.AddSingleton<DaemoClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery(); // Must be after Auth and before MapRazorComponents

app.MapControllers(); // 2. Add this line to enable attribute-based routing for APIs

app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
        .AddInteractiveWebAssemblyRenderMode()
  .AddAdditionalAssemblies(typeof(DaemoNETDemo.Client._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
