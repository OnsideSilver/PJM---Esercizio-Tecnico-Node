using Node_ApiService_Test.Controllers.ControllerExtensions;
using Node_ApiService_Test.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Register the custom schema filter
    options.SchemaFilter<SwaggerIgnoreFilter>();
});

builder.Services.AddSingleton<ProductService>();  // Register ProductService
builder.Services.AddSingleton<UserService>();     // Register UserService


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseAuthorization();

app.MapControllers();

app.Run();
