FROM mcr.microsoft.com/dotnet/sdk:9.0 AS compile
ENV APP_VERSION=2.0
WORKDIR /app
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /bin

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
LABEL maintainer="Max"
WORKDIR /release
COPY --from=compile /bin .
RUN useradd -m aspapp
USER aspapp
ENTRYPOINT [ "dotnet", "AspApp.dll" ]