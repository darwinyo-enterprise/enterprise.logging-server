FROM microsoft/aspnetcore:2.0-nanoserver-sac2016 AS base
WORKDIR /app
EXPOSE 5001

FROM microsoft/aspnetcore-build:2.0-nanoserver-sac2016 AS build
WORKDIR /src
COPY *.sln ./
COPY Logging Server/Enterprise.LoggingServer/Enterprise.LoggingServer.csproj Logging Server/Enterprise.LoggingServer/
RUN dotnet restore
COPY . .
WORKDIR /src/Logging Server/Enterprise.LoggingServer
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Enterprise.LoggingServer.dll"]
