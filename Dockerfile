FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
#FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY Core_RBS/*.csproj ./Core_RBS/
WORKDIR /app/Core_RBS
RUN dotnet restore

# copy and publish app and libraries
WORKDIR /app/
COPY Core_RBS/. ./Core_RBS/
WORKDIR /app/Core_RBS
RUN dotnet publish -c Release -o out
# RUN dotnet publish -c Release -r linux-arm -o out --self-contained true /p:PublishTrimmed=true


# FROM mcr.microsoft.com/dotnet/core/runtime-deps:3.0-buster-slim-arm32v7 AS runtime
FROM mcr.microsoft.com/dotnet/core/runtime:3.0 AS runtime
WORKDIR /app
COPY --from=build /app/Core_RBS/out ./
ENTRYPOINT ["dotnet", "Core_RBS.dll"]