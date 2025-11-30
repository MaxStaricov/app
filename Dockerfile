FROM mcr.microsoft.com/dotnet/sdk:9.0 AS compile
ENV APP_VERSION=1.2
ENV PORT=80
WORKDIR /app
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /bin

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
ENV PORT=80
USER app
LABEL maintainer="Max"
WORKDIR /release
COPY --from=compile /bin .
ENTRYPOINT [ "dotnet", "app.dll" ]