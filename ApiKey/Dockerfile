FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ApiKey/ApiKey.csproj", "ApiKey/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["DataAccess/DataAccess.csproj", "DataAccess/"]
COPY ["Domain/Domain.csproj", "Domain/"]
RUN dotnet restore "ApiKey/ApiKey.csproj"
COPY . .
WORKDIR "/src/ApiKey"
RUN dotnet build "./ApiKey.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ApiKey.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiKey.dll"]
