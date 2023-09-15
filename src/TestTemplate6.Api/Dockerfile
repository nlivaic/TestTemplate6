#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
# Uncomment if you have other certs this services needs to trust.
#COPY nginx/id-local.crt /usr/local/share/ca-certificates/id-local.crt
#RUN update-ca-certificates
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/TestTemplate6.Api/TestTemplate6.Api.csproj", "src/TestTemplate6.Api/"]
COPY ["src/TestTemplate6.Data/TestTemplate6.Data.csproj", "src/TestTemplate6.Data/"]
COPY ["src/TestTemplate6.Common/TestTemplate6.Common.csproj", "src/TestTemplate6.Common/"]
COPY ["src/TestTemplate6.Core/TestTemplate6.Core.csproj", "src/TestTemplate6.Core/"]
RUN dotnet restore "src/TestTemplate6.Api/TestTemplate6.Api.csproj"
COPY . .
WORKDIR "/src/src/TestTemplate6.Api"
RUN dotnet build "TestTemplate6.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestTemplate6.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestTemplate6.Api.dll"]