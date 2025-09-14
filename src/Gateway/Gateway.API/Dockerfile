# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project file and restore
COPY Gateway/Gateway.API/Gateway.API.csproj Gateway/Gateway.API/

# Restore dependencies
RUN dotnet restore Gateway/Gateway.API/Gateway.API.csproj

# Copy everything and build
COPY . .

WORKDIR /src/Gateway/Gateway.API
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "Gateway.API.dll"]
