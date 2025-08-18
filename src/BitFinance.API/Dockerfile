FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
ENV ASPNETCORE_URLS=http://0.0.0.0:8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["BitFinance.API/BitFinance.API.csproj", "BitFinance.API/"]
COPY ["BitFinance.Business/BitFinance.Business.csproj", "BitFinance.Business/"]
COPY ["BitFinance.Data/BitFinance.Data.csproj", "BitFinance.Data/"]
RUN dotnet restore "BitFinance.API/BitFinance.API.csproj"

COPY . .
WORKDIR "/src/BitFinance.API"
RUN dotnet build "BitFinance.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BitFinance.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BitFinance.API.dll"]