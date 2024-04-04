#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_URLS=http://+:8000;http://+:80;
ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MovieApp.AuthApi.API/MovieApp.AuthApi.API.csproj", "MovieApp.AuthApi.API/"]
COPY ["MovieApp.AuthApi.Identity/MovieApp.AuthApi.Identity.csproj", "MovieApp.AuthApi.Identity/"]
RUN dotnet restore "MovieApp.AuthApi.API/MovieApp.AuthApi.API.csproj"
COPY . .
WORKDIR "/src/MovieApp.AuthApi.API"
RUN dotnet build "MovieApp.AuthApi.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MovieApp.AuthApi.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MovieApp.AuthApi.API.dll"]