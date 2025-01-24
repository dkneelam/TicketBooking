var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        // Set the GuidRepresentation to Standard for correct Guid serialization
        MongoDB.Bson.Serialization.BsonSerializer.RegisterSerializer(new MongoDB.Bson.Serialization.Serializers.GuidSerializer(MongoDB.Bson.GuidRepresentation.Standard));
    });
builder.Services.AddEndpointsApiExplorer();

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")  // Allow the Angular app running on localhost:4200
              .AllowAnyHeader()  // Allow any headers
              .AllowAnyMethod(); // Allow any HTTP method (GET, POST, PUT, DELETE, etc.)
    });
});

var app = builder.Build();

// Use CORS policy for requests from Angular frontend
app.UseCors("AllowAngularApp");

app.UseAuthorization();

// Map controllers for API endpoints
app.MapControllers();

app.Run();
