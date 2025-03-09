FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["backend/LazyRisparmioforo/LazyRisparmioforo.Api/LazyRisparmioforo.Api.csproj", "backend/LazyRisparmioforo/LazyRisparmioforo.Api/"]
COPY ["backend/LazyRisparmioforo/LazyRisparmioforo.Infrastructure/LazyRisparmioforo.Infrastructure.csproj", "backend/LazyRisparmioforo/LazyRisparmioforo.Infrastructure/"]
COPY ["backend/LazyRisparmioforo/LazyRisparmioforo.Domain/LazyRisparmioforo.Domain.csproj", "backend/LazyRisparmioforo/LazyRisparmioforo.Domain/"]
COPY ["backend/LazyRisparmioforo/LazyRisparmioforo.Shared/LazyRisparmioforo.Shared/LazyRisparmioforo.Shared.csproj", "backend/LazyRisparmioforo/LazyRisparmioforo.Shared/LazyRisparmioforo.Shared/"]
COPY ["backend/LazyRisparmioforo/LazyRisparmioforo.Services/CategoryService/CategoryService.csproj", "backend/LazyRisparmioforo/LazyRisparmioforo.Services/CategoryService/"]
COPY ["backend/LazyRisparmioforo/LazyRisparmioforo.Services/ImportCsvService/ImportCsvService.csproj", "backend/LazyRisparmioforo/LazyRisparmioforo.Services/ImportCsvService/"]
COPY ["backend/LazyRisparmioforo/LazyRisparmioforo.Services/StatisticsService/StatisticsService.csproj", "backend/LazyRisparmioforo/LazyRisparmioforo.Services/StatisticsService/"]
COPY ["backend/LazyRisparmioforo/LazyRisparmioforo.Services/TransactionService/TransactionService.csproj", "backend/LazyRisparmioforo/LazyRisparmioforo.Services/TransactionService/"]
RUN dotnet restore "backend/LazyRisparmioforo/LazyRisparmioforo.Api/LazyRisparmioforo.Api.csproj"
COPY . .
WORKDIR "/src/backend/LazyRisparmioforo/LazyRisparmioforo.Api"
RUN dotnet build "LazyRisparmioforo.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "LazyRisparmioforo.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LazyRisparmioforo.Api.dll"]
