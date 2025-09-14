# ----------------------------
# 🔨 Build stage
# ----------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ./NuGet.Config ./NuGet.Config
# Copy project references (giữ nguyên cấu trúc thư mục)
COPY Shared/SharedKernel/SharedKernel.csproj Shared/SharedKernel/
COPY Shared/Shared.Auth/Shared.Auth.csproj Shared/Shared.Auth/
COPY EduService/EduService.Domain/EduService.Domain.csproj EduService/EduService.Domain/
COPY EduService/EduService.Infrastructure/EduService.Infrastructure.csproj EduService/EduService.Infrastructure/
COPY EduService/EduService.Application/EduService.Application.csproj EduService/EduService.Application/
COPY EduService/EduService.API/EduService.API.csproj EduService/EduService.API/

# Restore dependencies
RUN dotnet restore EduService/EduService.API/EduService.API.csproj --configfile NuGet.Config

# Copy all sources
COPY . .

# Publish app
WORKDIR /src/EduService/EduService.API
RUN dotnet publish -c Release -o /app/publish 

# ----------------------------
# 🚀 Runtime stage
# ----------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "EduService.API.dll"]
