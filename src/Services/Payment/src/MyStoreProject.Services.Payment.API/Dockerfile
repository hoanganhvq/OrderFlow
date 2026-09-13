FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/Payment/src/MyStoreProject.Services.Payment.API/MyStoreProject.Services.Payment.API.csproj", "src/Services/Payment/src/MyStoreProject.Services.Payment.API/"]
COPY ["src/Services/Payment/src/MyStoreProject.Services.Payment.Application/MyStoreProject.Services.Payment.Application.csproj", "src/Services/Payment/src/MyStoreProject.Services.Payment.Application/"]
COPY ["src/Services/Payment/src/MyStoreProject.Services.Payment.Domain/MyStoreProject.Services.Payment.Domain.csproj", "src/Services/Payment/src/MyStoreProject.Services.Payment.Domain/"]
COPY ["src/BuildingBlocks/BuildingBlocks.Common/BuildingBlocks.Common.csproj", "src/BuildingBlocks/BuildingBlocks.Common/"]
COPY ["src/Shared/MyStoreProject.Contracts/MyStoreProject.Contracts.csproj", "src/Shared/MyStoreProject.Contracts/"]
COPY ["src/Services/Payment/src/MyStoreProject.Services.Payment.Infrastructure/MyStoreProject.Services.Payment.Infrastructure.csproj", "src/Services/Payment/src/MyStoreProject.Services.Payment.Infrastructure/"]
RUN dotnet restore "src/Services/Payment/src/MyStoreProject.Services.Payment.API/MyStoreProject.Services.Payment.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Payment/src/MyStoreProject.Services.Payment.API"
RUN dotnet build "./MyStoreProject.Services.Payment.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MyStoreProject.Services.Payment.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyStoreProject.Services.Payment.API.dll"]
