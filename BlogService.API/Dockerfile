#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BlogService.API/BlogService.API.csproj", "BlogService.API/"]
COPY ["BlogService.Core/BlogService.Core.csproj", "BlogService.Core/"]
COPY ["BlogService.DataAccess/BlogService.DataAccess.csproj", "BlogService.DataAccess/"]
COPY ["BlogService.Models/BlogService.Models.csproj", "BlogService.Models/"]
COPY ["BlogService.Shared/BlogService.Shared.csproj", "BlogService.Shared/"]
RUN dotnet restore "BlogService.API/BlogService.API.csproj"
COPY . .
WORKDIR "/src/BlogService.API"
RUN dotnet build "BlogService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlogService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlogService.API.dll"]