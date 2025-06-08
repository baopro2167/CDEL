# S? d?ng .NET SDK image ?? build ?ng d?ng
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Sao ch�p c�c file d? �n v�o container
COPY ["WebApplication1/WebApplication1.csproj", "WebApplication1/"]
COPY ["Services/Services.csproj", "Services/"]
COPY ["Repositories/Repositories.csproj", "Repositories/"]
COPY ["Model/Model.csproj", "Model/"]
RUN dotnet restore "./WebApplication1/WebApplication1.csproj"

# Sao ch�p to�n b? m� ngu?n
COPY . .

# Build v� publish ?ng d?ng
WORKDIR "/src/WebApplication1"
RUN dotnet build "./WebApplication1.csproj" -c Release -o /app/build
RUN dotnet publish "./WebApplication1.csproj" -c Release -o /app/publish /p:UseAppHost=false

# S? d?ng .NET runtime image ?? ch?y ?ng d?ng
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WebApplication1.dll"]
