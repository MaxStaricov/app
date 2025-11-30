FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish -c Release -o Release

FROM mcr.microsoft.com/dotnet/sdk:9.0
WORKDIR /app
COPY --from=build /app/Release ./
LABEL MAINTAINER = "Стариков Максим, 89049946851"
ENTRYPOINT [ "dotnet", "app.dll" ]
