FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS base

RUN apt-get update && apt-get -y install unzip
RUN curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg	

RUN apt-get update && apt-get -y install procps

WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
ARG configuration=Release
WORKDIR /src

COPY *.csproj .
RUN dotnet restore

COPY . .

RUN dotnet build -c $configuration --no-restore

FROM build AS publish
ARG configuration=Release

RUN dotnet publish "DevelopersDays.Front.csproj" -c $configuration -o /app --no-build

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DevelopersDays.Front.dll"]