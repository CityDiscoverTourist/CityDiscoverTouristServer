ARG REPO=mcr.microsoft.com/dotnet/runtime


#FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
#WORKDIR /app
#EXPOSE 80
#EXPOSE 443

FROM $REPO:6.0.7-focal-amd64 AS build


ENV \
    # Unset ASPNETCORE_URLS from aspnet base image
    ASPNETCORE_URLS=http://+:80   \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Do not show first run text
    DOTNET_NOLOGO=true \
    # SDK version
    DOTNET_SDK_VERSION=6.0.302 \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-Ubuntu-20.04

# FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /app

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        curl \
        git \
        wget \
    && rm -rf /var/lib/apt/lists/*

RUN wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb
RUN rm packages-microsoft-prod.deb

RUN apt-get update; \
   apt-get install -y apt-transport-https && \
   apt-get update && \
   apt-get install -y dotnet-sdk-6.0 && \
   apt-get install -y aspnetcore-runtime-6.0

WORKDIR /src
COPY ["CityDiscoverTourist.API/CityDiscoverTourist.API.csproj", "CityDiscoverTourist.API/"]
COPY ["CityDiscoverTourist.Business/CityDiscoverTourist.Business.csproj", "CityDiscoverTourist.Business/"]
COPY ["CityDiscoverTourist.Data/CityDiscoverTourist.Data.csproj", "CityDiscoverTourist.Data/"]
RUN dotnet add "CityDiscoverTourist.API/CityDiscoverTourist.API.csproj"  package Emgu.CV.runtime.ubuntu.20.04-x64 --version 4.5.5.4823
RUN dotnet restore "CityDiscoverTourist.API/CityDiscoverTourist.API.csproj"
COPY . .
WORKDIR "/src/CityDiscoverTourist.API"
RUN dotnet build "CityDiscoverTourist.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CityDiscoverTourist.API.csproj" -c Release -o /app/publish

FROM build AS final

WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "CityDiscoverTourist.API.dll"]

