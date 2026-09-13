FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/Ordering/src/MyStoreProject.Services.Ordering.API/MyStoreProject.Services.Ordering.API.csproj", "src/Services/Ordering/src/MyStoreProject.Services.Ordering.API/"]
COPY ["src/Services/Ordering/src/MyStoreProject.Services.Ordering.Application/MyStoreProject.Services.Ordering.Application.csproj", "src/Services/Ordering/src/MyStoreProject.Services.Ordering.Application/"]
COPY ["src/Services/Ordering/src/MyStoreProject.Services.Ordering.Domain/MyStoreProject.Services.Ordering.Domain.csproj", "src/Services/Ordering/src/MyStoreProject.Services.Ordering.Domain/"]
COPY ["src/Services/Ordering/src/MyStoreProject.Services.Ordering.Infrastructure/MyStoreProject.Services.Ordering.Infrastructure.csproj", "src/Services/Ordering/src/MyStoreProject.Services.Ordering.Infrastructure/"]
COPY ["src/BuildingBlocks/BuildingBlocks.Common/BuildingBlocks.Common.csproj", "src/BuildingBlocks/BuildingBlocks.Common/"]
COPY ["src/Shared/MyStoreProject.Contracts/MyStoreProject.Contracts.csproj", "src/Shared/MyStoreProject.Contracts/"]
RUN dotnet restore "src/Services/Ordering/src/MyStoreProject.Services.Ordering.API/MyStoreProject.Services.Ordering.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Ordering/src/MyStoreProject.Services.Ordering.API"
RUN dotnet build "./MyStoreProject.Services.Ordering.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MyStoreProject.Services.Ordering.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyStoreProject.Services.Ordering.API.dll"]