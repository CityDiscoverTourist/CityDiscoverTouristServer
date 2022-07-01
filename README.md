# City-Discover-Tourist-Server


# Introduction
## City-Discover-Tourist-Server

[![ASP.NET](https://github.com/CityDiscoverTourist/CityDiscoverTouristServer/actions/workflows/dotnet.yml/badge.svg)](https://github.com/CityDiscoverTourist/CityDiscoverTouristServer/actions/workflows/dotnet.yml)
 [![CI-CD](https://github.com/CityDiscoverTourist/CityDiscoverTouristServer/actions/workflows/build.yml/badge.svg)](https://github.com/CityDiscoverTourist/CityDiscoverTouristServer/actions/workflows/build.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=CityDiscoverTourist_CityDiscoverTouristServer&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=CityDiscoverTourist_CityDiscoverTouristServer)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=CityDiscoverTourist_CityDiscoverTouristServer&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=CityDiscoverTourist_CityDiscoverTouristServer)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=CityDiscoverTourist_CityDiscoverTouristServer&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=CityDiscoverTourist_CityDiscoverTouristServer)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](https://makeapullrequest.com)
[![Hits](https://hits.seeyoufarm.com/api/count/incr/badge.svg?url=https%3A%2F%2Fgithub.com%2FCityDiscoverTourist%2FCityDiscoverTouristServer&count_bg=%2379C83D&title_bg=%23555555&icon=&icon_color=%23E7E7E7&title=hits&edge_flat=false)](https://hits.seeyoufarm.com)
[![DeepSource](https://deepsource.io/gh/CityDiscoverTourist/CityDiscoverTouristServer.svg/?label=active+issues&show_trend=true&token=MnPXbXl7_H-3A0Kt-cNOzrrj)](https://deepsource.io/gh/CityDiscoverTourist/CityDiscoverTouristServer/?ref=repository-badge)
## :ledger: Index

- [About](#beginner-about)
- [Usage](#zap-usage)
  - [Installation](#electric_plug-installation)
  - [Commands](#package-commands)
- [Development](#wrench-development)
  - [Pre-Requisites](#notebook-pre-requisites)
  - [Developmen Environment](#nut_and_bolt-development-environment)
  - [File Structure](#file_folder-file-structure)
  - [Build](#hammer-build)  
  - [Deployment](#rocket-deployment)  
- [Community](#cherry_blossom-community)
  - [Contribution](#fire-contribution)
  - [Guideline](#exclamation-guideline)  
- [Resources](#page_facing_up-resources)
- [Credit/Acknowledgment](#star2-creditacknowledgment)
- [License](#lock-license)

##  :beginner: About
- An API server for a platform that allows tourists to buy Quests designed by experienced people. Quests are self-guided tours combined with interesting puzzle games on the trip.
## :zap: Usage

###  :electric_plug: Installation

###  :package: Commands
- Commands to start the project.
```
git clone https://github.com/CityDiscoverTourist/CityDiscoverTouristServer.git
dotnet restore
dotnet build -c Release
dotnet dotnet test -c Release --no-build --verbosity normal
dotnet run
```

##  :wrench: Development

### :notebook: Pre-Requisites
List all the pre-requisites the system needs to develop this project.
- MSSQL
- Dotnet runtime
- IDE

###  :nut_and_bolt: Development Environment
Write about setting up the working environment for your project.
- How to download the project...
- How to install dependencies...


###  :file_folder: File Structure
Add a file structure here with the basic details about files, below is an example.

```
.
├── CityDiscoverTourist.API
│   ├── AWS
│   ├── Cache
│   ├── Config
│   │    ├── ConfigController.cs
│   │    ├── ConfigDatabase.cs
│   │    └── ConfigFirebase.cs
│   ├── Controllers
│   │    ├── AuthController.cs
│   │    ├── ContactController.cs
│   │    └── etc
│   ├── Response
│   │    └── ApiResponse.cs
│   ├── appsettings.json
│   ├── Program.cs
│   └── .gitignore
├── CityDiscoverTourist.Business
│   ├── Data
│   │   ├── RequestModel
|   │   │   ├── QuestRequestModel.cs
|   |   |   └── etc
│   │   ├── ResponseModel
|   │   │   ├── QuestResponseModel.cs
|   |   |   └── etc
│   │   └── AutoMapperProfile.cs
│   ├── Enum
│   ├── Exceptions
│   ├── Helper
│   │   ├── EmailHelper
│   │   ├── Params
|   │   │   ├── QueryStringParams.cs
|   │   │   ├── QuestParams.cs
|   │   │   └── etc
│   │   ├── ISortHelper.cs
│   │   ├── PageList.cs
│   │   └── SortHelper.cs
│   ├── IService
│   │   ├── Service
|   │   │   ├── AuthService.cs
|   │   │   ├── QuestService.cs
|   │   │   └── etc
│   │   ├── IAuthService.cs
│   │   ├── IQuestService.cs
│   │   └── etc
│   ├── Settings
│   └── .gitignore
├── CityDiscoverTourist.Data
│   ├── IRepositories
│   │   ├── Repositories
|   │   │   ├── IAnswerRepository.cs
|   │   │   └── etc
│   ├── Migrations
│   ├── Models 
│   ├── .gitignore 
├── CityDiscoverTourist.Test
└── README.md
```

###  :hammer: Build
```
dotnet build -c Release
```
### :rocket: Deployment
Write the deployment instruction here.

 ###  :fire: Contribution


 Your contributions are always welcome and appreciated. Following are the things you can do to contribute to this project.

##  :page_facing_up: Resources
Add important resources here


## :star2: Credit/Acknowledgment
### Contributors
-

##  :lock: License
License under the [MIT LICENSE](LICENSE)

