FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["BitFinance.WorkerService/BitFinance.WorkerService.csproj", "BitFinance.WorkerService/"]
COPY ["BitFinance.Business/BitFinance.Business.csproj", "BitFinance.Business/"]
COPY ["BitFinance.Data/BitFinance.Data.csproj", "BitFinance.Data/"]
RUN dotnet restore "BitFinance.WorkerService/BitFinance.WorkerService.csproj"


COPY . .
WORKDIR "/src/BitFinance.WorkerService"
RUN dotnet build "BitFinance.WorkerService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BitFinance.WorkerService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BitFinance.WorkerService.dll"]