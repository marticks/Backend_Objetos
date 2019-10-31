FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /WebApiObjetos

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /WebApiObjetos
COPY --from=build-env /WebApiObjetos/out .
ENTRYPOINT ["dotnet", "WebApiObjetos.dll"]