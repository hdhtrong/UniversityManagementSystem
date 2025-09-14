# ----------------------------
# 🔨 Build stage
# ----------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ./NuGet.Config ./NuGet.Config

# Copy project references (SharedKernel, AuthService nếu có dùng)
COPY Shared/SharedKernel/SharedKernel.csproj Shared/SharedKernel/
COPY Shared/Shared.Auth/Shared.Auth.csproj Shared/Shared.Auth/

# Copy NotificationService projects
COPY NotificationService/NotificationService.API/NotificationService.API.csproj NotificationService/NotificationService.API/

# Restore dependencies
RUN dotnet restore NotificationService/NotificationService.API/NotificationService.API.csproj --configfile NuGet.Config

# Copy toàn bộ mã nguồn
COPY . .

# Publish ứng dụng
WORKDIR /src/NotificationService/NotificationService.API
RUN dotnet publish -c Release -o /app/publish

# ----------------------------
# 🚀 Runtime stage
# ----------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "NotificationService.API.dll"]
