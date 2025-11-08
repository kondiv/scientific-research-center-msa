FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/services/ScientificReportService/ScientificReportService.App/ScientificReportService.App.csproj", "src/services/ScientificReportService/ScientificReportService.App/"]
COPY ["src/shared/Shared/Shared.ResultPattern/Shared.ResultPattern.csproj", "src/shared/Shared/Shared.ResultPattern/"]
COPY ["src/shared/Shared/Shared.Contracts/Shared.Contracts.csproj", "src/shared/Shared/Shared.Contracts/"]
RUN dotnet restore "src/services/ScientificReportService/ScientificReportService.App/ScientificReportService.App.csproj"
COPY . .
WORKDIR "/src/src/services/ScientificReportService/ScientificReportService.App"
RUN dotnet build "./ScientificReportService.App.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ScientificReportService.App.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ScientificReportService.App.dll"]
