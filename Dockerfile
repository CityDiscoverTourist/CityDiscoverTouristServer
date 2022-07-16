ARG REPO=mcr.microsoft.com/dotnet/runtime


#FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
#WORKDIR /app
#EXPOSE 80
#EXPOSE 443

FROM $REPO:6.0.7-focal-amd64 AS build

WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV \
    # Unset ASPNETCORE_URLS from aspnet base image
    ASPNETCORE_URLS= \
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

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        curl \
        git \
        wget \
    && rm -rf /var/lib/apt/lists/*

# install System.Drawing native dependencies
RUN apt-get update
RUN apt-get install -y libicu-dev libharfbuzz0b libfontconfig1 libfreetype6
RUN apt-get install -y libgdiplus libx11-dev libgeotiff-dev  libxt-dev libopengl-dev libglx-dev libusb-1.0-0

RUN curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-x64.tar.gz \
    && dotnet_sha512='ac1d124802ca035aa00806312460b371af8e3a55d85383ddd8bb66f427c4fabae75b8be23c45888344e13b283a4f9c7df228447c06d796a57ffa5bb21992e6a4' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet ./packs ./sdk ./sdk-manifests ./templates ./LICENSE.txt ./ThirdPartyNotices.txt \
    && rm dotnet.tar.gz \
    # Trigger first run experience by running arbitrary cmd
    && dotnet help

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
COPY --from=publish /app/publish .



ENTRYPOINT ["dotnet", "CityDiscoverTourist.API.dll"]

