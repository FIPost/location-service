![ipost-logo](https://github.com/FIPost/docs/blob/master/assets/logo-name.png?raw=true)
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

#### Error: Docker Network Missing
If you get the following error:
Network `ipost-network` declared as external, but could not be found. Run the following:
```zsh
docker network create ipost-network
```
