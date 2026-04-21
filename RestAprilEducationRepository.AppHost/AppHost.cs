var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.RestAprilEducationRepository_API>("restaprileducationrepository-api");

builder.AddProject<Projects.RestAprilEducationRepository_RazorPages>("restaprileducationrepository-razorpages");

builder.Build().Run();
