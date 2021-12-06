
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("http://localhost:3000",
                                "http://www.contoso.com");
        });
});

// Add services to the container.
//Dependency injection
builder.Services.AddSingleton<CMCOnlineStoreApi.IProductRepository, CMCOnlineStoreApi.ProductRepository>();
builder.Services.AddSingleton<BusinessLogic.IBL, BusinessLogic.BL>();
//Caching
builder.Services.AddMemoryCache();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
