var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddEndpoints(typeof(Program).Assembly);

builder.Services.AddHttpClient<AuctionServiceHttpClient>().AddPolicyHandler(GetPolicy());

builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();

    configurator.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search", false));

    configurator.UsingRabbitMq((context, cfg) =>
    {
        cfg.ReceiveEndpoint("search-auction-created", e =>
        {
            e.UseMessageRetry(r => r.Interval(5, 5));

            e.ConfigureConsumer<AuctionCreatedConsumer>(context);

            e.ConfigureConsumer<AuctionUpdatedConsumer>(context);

            e.ConfigureConsumer<AuctionDeletedConsumer>(context);
        });

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.Lifetime.ApplicationStarted.Register(async void () =>
{
    try
    {
        await DbInitializer.InitDb(app);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
});

app.MapEndpoints();

app.Run();

return;

static AsyncRetryPolicy<HttpResponseMessage> GetPolicy() =>
    HttpPolicyExtensions.HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3));