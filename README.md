![ipost-logo](https://github.com/FIPost/docs/blob/master/assets/logo-name.png?raw=true)

[![Build .NET API](https://github.com/FIPost/locatieservice/actions/workflows/build.yml/badge.svg)](https://github.com/FIPost/locatieservice/actions/workflows/build.yml)
[![Docker Publish](https://github.com/FIPost/locatieservice/actions/workflows/docker-publish.yml/badge.svg)](https://github.com/FIPost/locatieservice/actions/workflows/docker-publish.yml)

# Location Service
<h3 align="center">
  <a href="https://github.com/FIPost/docs">Documentation</a>
</h3>


## Getting Started
```zsh
dotnet build
```
```zsh
dotnet restore
```
```zsh
dotnet run
```

## Run with Docker
```
docker-compose up --build
```

### Run external database
Set your own database by editing the connectionstring in `appsettings.json`. <br/>
Then run:
```zsh
docker run -p 5002:5002 --name location-service-app location-service
```

#### Error: Docker Network Missing
If you get the following error:
Network `ipost-network` declared as external, but could not be found. Run the following:
```zsh
docker network create ipost-network
```
