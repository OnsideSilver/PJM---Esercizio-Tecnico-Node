using Microsoft.OpenApi.Models;
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
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "JMP_Node_ApiService_Test", Version = "v1" });
});

builder.Services.AddSingleton<ProductService>();  // Register ProductService
builder.Services.AddSingleton<UserService>();     // Register UserService


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "JMP_Node_ApiService_Test V1");
        c.RoutePrefix = "swagger";
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
