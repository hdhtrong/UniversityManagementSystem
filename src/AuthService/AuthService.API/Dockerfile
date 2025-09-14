# ----------------------------
# 🔨 Build stage
# ----------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ./NuGet.Config ./NuGet.Config
# Copy csproj files
COPY Shared/SharedKernel/SharedKernel.csproj Shared/SharedKernel/
COPY Shared/Shared.Auth/Shared.Auth.csproj Shared/Shared.Auth/
COPY AuthService/AuthService.Domain/AuthService.Domain.csproj AuthService/AuthService.Domain/
COPY AuthService/AuthService.Infrastructure/AuthService.Infrastructure.csproj AuthService/AuthService.Infrastructure/
COPY AuthService/AuthService.Application/AuthService.Application.csproj AuthService/AuthService.Application/
COPY AuthService/AuthService.API/AuthService.API.csproj AuthService/AuthService.API/

# Restore dependencies
RUN dotnet restore AuthService/AuthService.API/AuthService.API.csproj --configfile NuGet.Config

# Copy all source
COPY . .

# Publish
WORKDIR /src/AuthService/AuthService.API
RUN dotnet publish -c Release -o /app/publish

# ----------------------------
# 🚀 Runtime stage
# ----------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "AuthService.API.dll"]
