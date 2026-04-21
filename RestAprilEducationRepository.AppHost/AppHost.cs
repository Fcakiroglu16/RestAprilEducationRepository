var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.RestAprilEducationRepository_API>("restaprileducationrepository-api");

builder.Build().Run();
