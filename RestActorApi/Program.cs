using ActorRepositoryLib;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAll",
                              policy =>
                              {
                                  policy.WithOrigins("http://zealand.dk").WithMethods("GET", "POST", "PUT", "DELETE").AllowAnyHeader();
                              });
});


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IActorsRepository>( new ActorsRepository()
    );

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");
app.Run();
