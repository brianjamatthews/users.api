#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Users.Api/Users.Api.csproj", "src/Users.Api/"]
COPY ["src/Users.ApplicationCore/Users.ApplicationCore.csproj", "src/Users.ApplicationCore/"]
COPY ["src/Users.Infrastructure/Users.Infrastructure.csproj", "src/Users.Infrastructure/"]
RUN dotnet restore "src/Users.Api/Users.Api.csproj"
COPY . .
WORKDIR "/src/src/Users.Api"
RUN dotnet build "Users.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Users.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Users.Api.dll"]