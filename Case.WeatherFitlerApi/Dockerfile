#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Case.WeatherFitlerApi/Case.WeatherFitlerApi.csproj", "Case.WeatherFitlerApi/"]
RUN dotnet restore "Case.WeatherFitlerApi/Case.WeatherFitlerApi.csproj"
COPY . .
WORKDIR "/src/Case.WeatherFitlerApi"
RUN dotnet build "Case.WeatherFitlerApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Case.WeatherFitlerApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Case.WeatherFitlerApi.dll"]