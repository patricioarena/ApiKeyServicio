# Base stage
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ApiKey/ApiKey.csproj", "ApiKey/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["DataAccess/DataAccess.csproj", "DataAccess/"]
COPY ["Domain/Domain.csproj", "Domain/"]
RUN dotnet restore "ApiKey/ApiKey.csproj"

# Copy Resources directory
COPY ["ApiKey/Resources/", "ApiKey/Resources/"]
COPY . .
WORKDIR "/src/ApiKey"
RUN dotnet build "./ApiKey.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ApiKey.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

# Install SQLite and set up directories
RUN apt-get update && \
    apt-get install -y --no-install-recommends \
    sqlite3 \
    libsqlite3-dev && \
    rm -rf /var/lib/apt/lists/*

# Create Resources directory and set permissions
RUN mkdir -p /app/Resources && \
    chmod 777 /app/Resources

# Copy published app and resources
COPY --from=publish /app/publish .
COPY ["ApiKey/Resources/", "/app/Resources/"]

# Set permissions for the SQLite database file
RUN touch /app/Resources/whitelist.db && \
    chmod 666 /app/Resources/whitelist.db

ENTRYPOINT ["dotnet", "ApiKey.dll"]
