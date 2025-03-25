var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

await DB.InitAsync("SearchDb",
    MongoClientSettings.FromConnectionString(builder.Configuration.GetConnectionString("MongoDbConnection")));

await DB.Index<Item>()
    .Key(i => i.Make, KeyType.Text)
    .Key(i => i.Model, KeyType.Text)
    .Key(i => i.Color, KeyType.Text)
    .CreateAsync();

app.Run();