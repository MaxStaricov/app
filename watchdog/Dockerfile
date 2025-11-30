FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /Watchdog
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /bin

FROM mcr.microsoft.com/dotnet/runtime:9.0
USER watchdog
WORKDIR /bin
COPY --from=build /bin .
ENTRYPOINT ["dotnet", "watchdog.dll"]
