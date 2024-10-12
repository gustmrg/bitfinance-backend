FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/BitFinance.API/BitFinance.API.csproj", "src/BitFinance.API/"]
COPY ["src/BitFinance.Business/BitFinance.Business.csproj", "src/BitFinance.Business/"]
COPY ["src/BitFinance.Data/BitFinance.Data.csproj", "src/BitFinance.Data/"]
RUN dotnet restore "src/BitFinance.API/BitFinance.API.csproj"
COPY . .
WORKDIR "/src/src/BitFinance.API"
RUN dotnet build "BitFinance.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BitFinance.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BitFinance.API.dll"]
