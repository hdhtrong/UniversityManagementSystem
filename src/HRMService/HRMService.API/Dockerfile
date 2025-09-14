# ----------------------------
# 🔨 Build stage
# ----------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ./NuGet.Config ./NuGet.Config
# Copy project references (giữ đúng cấu trúc thư mục)
COPY Shared/SharedKernel/SharedKernel.csproj Shared/SharedKernel/
COPY Shared/Shared.Auth/Shared.Auth.csproj Shared/Shared.Auth/
COPY HRMService/HRMService.Domain/HRMService.Domain.csproj HRMService/HRMService.Domain/
COPY HRMService/HRMService.Infrastructure/HRMService.Infrastructure.csproj HRMService/HRMService.Infrastructure/
COPY HRMService/HRMService.Application/HRMService.Application.csproj HRMService/HRMService.Application/
COPY HRMService/HRMService.API/HRMService.API.csproj HRMService/HRMService.API/

# Restore dependencies
RUN dotnet restore HRMService/HRMService.API/HRMService.API.csproj --configfile NuGet.Config

# Copy toàn bộ mã nguồn
COPY . .

# Publish ứng dụng
WORKDIR /src/HRMService/HRMService.API
RUN dotnet publish -c Release -o /app/publish

# ----------------------------
# 🚀 Runtime stage
# ----------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "HRMService.API.dll"]
