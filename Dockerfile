
FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY Core_RBS/*.csproj ./Core_RBS/
RUN dotnet restore

# copy everything else and build app
COPY Core_RBS/. ./Core_RBS/
WORKDIR /app/Core_RBS
RUN dotnet publish  -c Release -o out


FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app
COPY --from=build /app/Core_RBS/out ./

RUN apt-get update && apt-get install -y libc6-dev libgdiplus && rm -rf /var/lib/apt/lists/*

CMD ASPNETCORE_URLS=http://*:$PORT dotnet Core_RBS.dll
# ENTRYPOINT ["dotnet", "Core_RBS.dll"]
