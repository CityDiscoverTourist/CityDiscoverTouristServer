FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build


WORKDIR /src
COPY ["CityDiscoverTourist.API/CityDiscoverTourist.API.csproj", "CityDiscoverTourist.API/"]
COPY ["CityDiscoverTourist.Business/CityDiscoverTourist.Business.csproj", "CityDiscoverTourist.Business/"]
COPY ["CityDiscoverTourist.Data/CityDiscoverTourist.Data.csproj", "CityDiscoverTourist.Data/"]
RUN dotnet add "CityDiscoverTourist.API/CityDiscoverTourist.API.csproj"  package Emgu.CV.runtime.ubuntu.20.04-x64 --version 4.5.3.4721
RUN dotnet restore "CityDiscoverTourist.API/CityDiscoverTourist.API.csproj"
COPY . .
WORKDIR "/src/CityDiscoverTourist.API"
RUN dotnet build "CityDiscoverTourist.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CityDiscoverTourist.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# install System.Drawing native dependencies
RUN apt-get update
RUN apt-get install -y libgdiplus libc6-dev
RUN apt-get install -y libicu-dev libharfbuzz0b libfontconfig1 libfreetype6
RUN apt-get install -y \
    build-essential \
	libgtk-3-dev \
	libgstreamer1.0-dev \
	libavcodec-dev \
	libswscale-dev \
	libavformat-dev \
	libdc1394-22-dev \
	libv4l-dev \
	cmake-curses-gui \
	ocl-icd-dev \
	freeglut3-dev \
	libgeotiff-dev \
	libusb-1.0-0-dev 

ENTRYPOINT ["dotnet", "CityDiscoverTourist.API.dll"]

