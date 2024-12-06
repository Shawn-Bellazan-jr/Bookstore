using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache", 6379)
    .WithRedisInsight()
    .WithRedisCommander();

var apiService = builder.AddProject<Projects.Bookstore_ApiService>("apiservice");

builder.AddProject<Projects.Bookstore_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
