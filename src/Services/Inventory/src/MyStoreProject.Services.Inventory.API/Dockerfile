FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/Inventory/src/MyStoreProject.Services.Inventory.API/MyStoreProject.Services.Inventory.API.csproj", "src/Services/Inventory/src/MyStoreProject.Services.Inventory.API/"]
COPY ["src/Services/Inventory/src/MyStoreProject.Services.Inventory.Application/MyStoreProject.Services.Inventory.Application.csproj", "src/Services/Inventory/src/MyStoreProject.Services.Inventory.Application/"]
COPY ["src/Services/Inventory/src/MyStoreProject.Services.Inventory.Domain/MyStoreProject.Services.Inventory.Domain.csproj", "src/Services/Inventory/src/MyStoreProject.Services.Inventory.Domain/"]
COPY ["src/BuildingBlocks/BuildingBlocks.Common/BuildingBlocks.Common.csproj", "src/BuildingBlocks/BuildingBlocks.Common/"]
COPY ["src/Shared/MyStoreProject.Contracts/MyStoreProject.Contracts.csproj", "src/Shared/MyStoreProject.Contracts/"]
COPY ["src/Services/Inventory/src/MyStoreProject.Services.Inventory.Infrastructure/MyStoreProject.Services.Inventory.Infrastructure.csproj", "src/Services/Inventory/src/MyStoreProject.Services.Inventory.Infrastructure/"]
RUN dotnet restore "src/Services/Inventory/src/MyStoreProject.Services.Inventory.API/MyStoreProject.Services.Inventory.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Inventory/src/MyStoreProject.Services.Inventory.API"
RUN dotnet build "./MyStoreProject.Services.Inventory.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MyStoreProject.Services.Inventory.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyStoreProject.Services.Inventory.API.dll"]
