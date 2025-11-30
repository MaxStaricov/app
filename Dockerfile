# FROM mcr.microsoft.com/dotnet/sdk:9.0 AS compile_project
# WORKDIR /app
# COPY *.csproj ./
# RUN dotnet restore
# COPY . ./
# RUN dotnet publish -c Release -o /bin/Release

# FROM mcr.microsoft.com/dotnet/aspnet:9.0
# LABEL MAINTAINER = "Стариков Максим, 89049946851"
# ENV PORT=80
# ENV APP_VERSION=1.0
# USER app
# WORKDIR /app
# COPY --from=build /app/Release ./
# ENTRYPOINT [ "dotnet", "app.dll" ]

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
LABEL MAINTAINER="Max"
COPY --from=compile /bin .
ENTRYPOINT [ "dotnet", "app.dll" ]