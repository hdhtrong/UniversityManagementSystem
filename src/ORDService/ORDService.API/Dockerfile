# ----------------------------
# 🔨 Build stage
# ----------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ./NuGet.Config ./NuGet.Config
# Copy project references (giữ đúng cấu trúc thư mục)
COPY Shared/SharedKernel/SharedKernel.csproj Shared/SharedKernel/
COPY Shared/Shared.Auth/Shared.Auth.csproj Shared/Shared.Auth/
COPY ORDService/ORDService.Domain/ORDService.Domain.csproj ORDService/ORDService.Domain/
COPY ORDService/ORDService.Infrastructure/ORDService.Infrastructure.csproj ORDService/ORDService.Infrastructure/
COPY ORDService/ORDService.Application/ORDService.Application.csproj ORDService/ORDService.Application/
COPY ORDService/ORDService.API/ORDService.API.csproj ORDService/ORDService.API/

# Restore dependencies
RUN dotnet restore ORDService/ORDService.API/ORDService.API.csproj --configfile NuGet.Config

# Copy toàn bộ mã nguồn
COPY . .

# Publish ứng dụng
WORKDIR /src/ORDService/ORDService.API
RUN dotnet publish -c Release -o /app/publish

# ----------------------------
# 🚀 Runtime stage
# ----------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "ORDService.API.dll"]
