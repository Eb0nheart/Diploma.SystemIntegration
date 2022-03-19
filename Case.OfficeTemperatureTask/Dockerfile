#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Case.OfficeTemperatureTask/Case.OfficeTemperatureTask.csproj", "Case.OfficeTemperatureTask/"]
RUN dotnet restore "Case.OfficeTemperatureTask/Case.OfficeTemperatureTask.csproj"
COPY . .
WORKDIR "/src/Case.OfficeTemperatureTask"
RUN dotnet build "Case.OfficeTemperatureTask.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Case.OfficeTemperatureTask.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Case.OfficeTemperatureTask.dll"]