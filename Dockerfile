FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS base
WORKDIR /app
EXPOSE 7037

ENV ASPNETCORE_URLS=http://+:7037

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /src
COPY ./BdTracker.sln ./
COPY ./BdTracker.Back/BdTracker.Back.csproj ./BdTracker.Back/
COPY ./BdTracker.Shared/BdTracker.Shared.csproj ./BdTracker.Shared/

RUN dotnet restore
COPY . .

WORKDIR /src/BdTracker.Shared
RUN dotnet build -c Release -o /app

WORKDIR /src/BdTracker.Back
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
COPY ./BdTracker.Back/App_Data/bdtdatabase.db ./App_Data/
ENV "ConnectionStrings:ConnectionName"="Data Source=~/App_Data/bdtdatabase.db"
ENV "SecretAccess:Key"="Key"
ENV "Authentication:Key"="ThisIsVerySecureKeyAndNoBodyKnowedIt"
ENV "SecretAccess:Value"="Value"

ENTRYPOINT ["dotnet", "BdTracker.Back.dll"]