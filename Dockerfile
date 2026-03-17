# FASE DE RUNTIME (Para ejecutar la app)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# FASE DE BUILD (Para compilar el código)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Se copia solo el archivo de proyecto para restaurar dependencias
COPY ["TaskManagerApi.csproj", "."]
RUN dotnet restore "./TaskManagerApi.csproj"

# Se copia todo lo demás y compilamos
COPY . .
RUN dotnet build "TaskManagerApi.csproj" -c Release -o /app/build

# PUBLICACIÓN: Preparamos los archivos finales
FROM build AS publish
RUN dotnet publish "TaskManagerApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# IMAGEN FINAL: Limpia y ligera
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskManagerApi.dll"]