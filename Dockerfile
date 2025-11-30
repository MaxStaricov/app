FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish -c Release -o Release

FROM mcr.microsoft.com/dotnet/aspnet:9.0
LABEL MAINTAINER = "Стариков Максим, 89049946851"
ENV PORT=80
ENV APP_VERSION=1.0
USER app
WORKDIR /app
COPY --from=build /app/Release ./
ENTRYPOINT [ "dotnet", "app.dll" ]