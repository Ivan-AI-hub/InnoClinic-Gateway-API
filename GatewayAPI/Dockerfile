#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GatewayAPI.Web/GatewayAPI.Web.csproj", "GatewayAPI.Web/"]
RUN dotnet restore "GatewayAPI.Web/GatewayAPI.Web.csproj"
COPY . .
WORKDIR "/src/GatewayAPI.Web"
RUN dotnet build "GatewayAPI.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GatewayAPI.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GatewayAPI.Web.dll"]