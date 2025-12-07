FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TableService.Api/TableService.Api.csproj", "TableService.Api/"]
COPY ["Contract/Contract.csproj", "Contract/"]
COPY ["TableService.Application/TableService.Application.csproj", "TableService.Application/"]
COPY ["TableService.Domain/TableService.Domain.csproj", "TableService.Domain/"]
RUN dotnet restore "TableService.Api/TableService.Api.csproj"
COPY . .
WORKDIR "/src/TableService.Api"
RUN dotnet build "TableService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TableService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TableService.Api.dll"]
