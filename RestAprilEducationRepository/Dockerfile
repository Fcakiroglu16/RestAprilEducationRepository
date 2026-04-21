# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RestAprilEducationRepository/RestAprilEducationRepository.API.csproj", "RestAprilEducationRepository/"]
COPY ["RestAprilEducationRepository.Application/RestAprilEducationRepository.Application.csproj", "RestAprilEducationRepository.Application/"]
COPY ["RestAprilEducationRepository.Domain/RestAprilEducationRepository.Domain.csproj", "RestAprilEducationRepository.Domain/"]
COPY ["RestAprilEducationRepository.Persistence/RestAprilEducationRepository.Persistence.csproj", "RestAprilEducationRepository.Persistence/"]
RUN dotnet restore "./RestAprilEducationRepository/RestAprilEducationRepository.API.csproj"
COPY . .
WORKDIR "/src/RestAprilEducationRepository"
RUN dotnet build "./RestAprilEducationRepository.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RestAprilEducationRepository.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RestAprilEducationRepository.API.dll"]