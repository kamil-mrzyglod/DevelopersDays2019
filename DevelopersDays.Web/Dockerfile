FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base

RUN apt-get update && apt-get -y install unzip
RUN curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg	

WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src

COPY *.csproj .
RUN dotnet restore

COPY . .

RUN dotnet build -c Release --no-restore

FROM build AS publish
RUN dotnet publish "DevelopersDays.Web.csproj" -c Release -o /app --no-build

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DevelopersDays.Web.dll"]