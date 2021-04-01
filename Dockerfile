#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 5002

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["LocatieService.csproj", "."]
RUN dotnet restore "./LocatieService.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "LocatieService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LocatieService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LocatieService.dll"]