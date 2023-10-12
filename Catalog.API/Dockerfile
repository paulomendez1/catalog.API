FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /App

COPY src/Catalog.API/Catalog.API.csproj src/Catalog.API/
COPY src/Catalog.API/Catalog.API.sln src/Catalog.API/
RUN dotnet restore src/Catalog.API/Catalog.API.csproj

COPY . ./
RUN dotnet publish -c Release -o out src/Catalog.API/Catalog.API.csproj -v d

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "Catalog.API.dll"]