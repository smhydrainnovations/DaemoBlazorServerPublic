# DaemoBlazorServerPublic
A .NET Blazor application demonstrating the purpose and capability of Daemo AI SDK. The Daemo SDK NuGet package directly connects the .NET Application to the database via Daemo Functions with built-in Role Based Access Control. It makes it possible to prompt your database in Natural Language securely.


#program.cs


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

