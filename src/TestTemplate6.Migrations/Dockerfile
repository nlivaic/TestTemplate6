#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/TestTemplate6.Migrations/TestTemplate6.Migrations.csproj", "src/TestTemplate6.Migrations/"]
RUN dotnet restore "src/TestTemplate6.Migrations/TestTemplate6.Migrations.csproj"
COPY . .
WORKDIR "/src/src/TestTemplate6.Migrations"
RUN dotnet build "TestTemplate6.Migrations.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestTemplate6.Migrations.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestTemplate6.Migrations.dll"]