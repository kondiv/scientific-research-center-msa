FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/services/AuthenticationService/AuthenticationService.App/AuthenticationService.App.csproj", "src/services/AuthenticationService/AuthenticationService.App/"]
COPY ["src/shared/Shared/Shared.ResultPattern/Shared.ResultPattern.csproj", "src/shared/Shared/Shared.ResultPattern/"]
RUN dotnet restore "src/services/AuthenticationService/AuthenticationService.App/AuthenticationService.App.csproj"
COPY . .
WORKDIR "/src/src/services/AuthenticationService/AuthenticationService.App"
RUN dotnet build "./AuthenticationService.App.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AuthenticationService.App.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthenticationService.App.dll"]
