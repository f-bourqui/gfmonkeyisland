#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/MonkeyIsland.Core/*.csproj", "MonkeyIsland.Core/"]
COPY ["src/MonkeyIsland/*.csproj", "MonkeyIsland/"]

RUN dotnet restore "MonkeyIsland/MonkeyIsland.csproj"
COPY ./src .

WORKDIR "/src/MonkeyIsland.Core"
RUN dotnet build "MonkeyIsland.Core.csproj" -c Release -o /app/build

WORKDIR "/src/MonkeyIsland"
RUN dotnet build "MonkeyIsland.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MonkeyIsland.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MonkeyIsland.dll"]