FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /src
COPY ["Core_RBS/Core_RBS.csproj", "Core_RBS/"]
RUN dotnet restore "Core_RBS/Core_RBS.csproj"
COPY . .
WORKDIR "/src/Core_RBS"
RUN dotnet build "Core_RBS.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Core_RBS.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Core_RBS.dll"]