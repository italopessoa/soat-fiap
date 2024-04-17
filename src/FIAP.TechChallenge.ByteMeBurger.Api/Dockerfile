FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/FIAP.TechChallenge.ByteMeBurger.Api/FIAP.TechChallenge.ByteMeBurger.Api.csproj", "src/FIAP.TechChallenge.ByteMeBurger.Api/"]
RUN dotnet restore "src/FIAP.TechChallenge.ByteMeBurger.Api/FIAP.TechChallenge.ByteMeBurger.Api.csproj"
COPY . .
WORKDIR "/src/src/FIAP.TechChallenge.ByteMeBurger.Api"
RUN dotnet build "FIAP.TechChallenge.ByteMeBurger.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FIAP.TechChallenge.ByteMeBurger.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FIAP.TechChallenge.ByteMeBurger.Api.dll"]
