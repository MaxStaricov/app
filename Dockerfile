FROM mcr.microsoft.com/dotnet/sdk:9.0 AS compile
ENV APP_VERSION=2.0
WORKDIR /src
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /bin /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
USER AspApp
LABEL maintainer="Max"
WORKDIR /release
COPY --from=compile /bin .
ENTRYPOINT [ "dotnet", "AspApp.dll" ]