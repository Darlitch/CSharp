FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CoordinatorService.Host/CoordinatorService.Host.csproj", "CoordinatorService.Host/"]
COPY ["CoordinatorService.Application/CoordinatorService.Application.csproj", "CoordinatorService.Application/"]
COPY ["Contract/Contract.csproj", "Contract/"]
COPY ["CoordinatorService.Infrastructure/CoordinatorService.Infrastructure.csproj", "CoordinatorService.Infrastructure/"]
RUN dotnet restore "CoordinatorService.Host/CoordinatorService.Host.csproj"
COPY . .
WORKDIR "/src/CoordinatorService.Host"
RUN dotnet build "CoordinatorService.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CoordinatorService.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoordinatorService.Host.dll"]
