#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/TestTemplate6.WorkerServices/TestTemplate6.WorkerServices.csproj", "src/TestTemplate6.WorkerServices/"]
COPY ["src/TestTemplate6.Core/TestTemplate6.Core.csproj", "src/TestTemplate6.Core/"]
COPY ["src/TestTemplate6.Common/TestTemplate6.Common.csproj", "src/TestTemplate6.Common/"]
RUN dotnet restore "src/TestTemplate6.WorkerServices/TestTemplate6.WorkerServices.csproj"
COPY . .
WORKDIR "/src/src/TestTemplate6.WorkerServices"
RUN dotnet build "TestTemplate6.WorkerServices.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestTemplate6.WorkerServices.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["src/TestTemplate6.WorkerServices/entrypoint.sh", "src/entrypoint.sh"]
COPY ["src/TestTemplate6.WorkerServices/wait-for-it.sh", "src/wait-for-it.sh"]
CMD ["/bin/bash", "src/entrypoint.sh"]
