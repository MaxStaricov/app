FROM mcr.microsoft.com/dotnet/sdk:9.0 AS compile
ENV APP_VERSION=2.0
WORKDIR /src
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /bin

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
LABEL maintainer="Max"
WORKDIR /curl
RUN apt-get update && apt-get install -y curl
WORKDIR /bin
COPY --from=compile /bin .
RUN useradd -m aspapp
USER aspapp
ENTRYPOINT [ "dotnet", "AspApp.dll" ]