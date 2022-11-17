FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Florence.API/Florence.API.csproj", "Florence.API/"]
RUN dotnet restore "Florence.API/Florence.API.csproj"
COPY . .
WORKDIR "/src/Florence.API"
RUN dotnet build "Florence.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Florence.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Florence.API.dll"]
