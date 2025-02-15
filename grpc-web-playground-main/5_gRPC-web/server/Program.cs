using Microsoft.AspNetCore.Server.Kestrel.Core;
using server.Services;
using System.Net;
using Microsoft.AspNetCore.Cors.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpcReflection();
builder.Services.AddGrpc();
builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader()
           .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
}));

builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any, 5000, o => o.Protocols = HttpProtocols.Http1AndHttp2);
});

var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseCors("AllowAll"); // Ensure CORS is applied first


app.MapGrpcService<GreeterService>();
app.MapGrpcReflectionService();
app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
