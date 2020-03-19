ARG CONFIGURATION=Release
ARG DOCKERFILE_VERSION=1.0.0
ARG SDK=3.1.101
ARG RUNTIME=3.1

FROM mcr.microsoft.com/dotnet/core/sdk:${SDK} AS builder
ARG CONFIGURATION
ARG DOCKERFILE_VERSION
ARG NUGET_CONFIG_FILE=nuget.config
ENV NUGET_CONFIG_FILE=${NUGET_CONFIG_FILE}
LABEL dockerfile="version $DOCKERFILE_VERSION"

COPY ["./global.json", "./global.json"]
COPY ["./nuget.config", "./nuget.config"]
COPY ["./src/PT.Fibonacci.sln", "./PT.Fibonacci.sln"]
COPY ["./src/PT.Fibonacci.Api/PT.Fibonacci.Api.csproj", "./PT.Fibonacci.Api/PT.Fibonacci.Api.csproj"]
COPY ["./src/PT.Fibonacci.Client/PT.Fibonacci.Client.csproj", "./PT.Fibonacci.Client/PT.Fibonacci.Client.csproj"]
COPY ["./src/PT.Fibonacci.Contracts/PT.Fibonacci.Contracts.csproj", "./PT.Fibonacci.Contracts/PT.Fibonacci.Contracts.csproj"]
COPY ["./src/PT.Fibonacci.DataAccess/PT.Fibonacci.DataAccess.csproj", "./PT.Fibonacci.DataAccess/PT.Fibonacci.DataAccess.csproj"]
COPY ["./src/PT.Fibonacci.Logic/PT.Fibonacci.Logic.csproj", "./PT.Fibonacci.Logic/PT.Fibonacci.Logic.csproj"]

RUN dotnet restore --configfile $NUGET_CONFIG_FILE -v:minimal -warnaserror

COPY ./src .

RUN dotnet build --no-restore -c ${CONFIGURATION} -v:minimal
RUN dotnet publish --no-build -c ${CONFIGURATION} -o /app/api PT.Fibonacci.Api/PT.Fibonacci.Api.csproj
RUN dotnet publish --no-build -c ${CONFIGURATION} -o /app/client PT.Fibonacci.Client/PT.Fibonacci.Client.csproj

FROM mcr.microsoft.com/dotnet/core/aspnet:${RUNTIME} AS api
ARG DOCKERFILE_VERSION
ENV LC_ALL=en_US.UTF-8 \
    LANG=en_US.UTF-8 \
    LANGUAGE=en_US:en \
    TZ=UTC
WORKDIR /app/api
EXPOSE 80

COPY --from=builder /app /app

ENTRYPOINT ["dotnet", "PT.Fibonacci.Api.dll"]


FROM mcr.microsoft.com/dotnet/core/aspnet:${RUNTIME} AS client
ARG DOCKERFILE_VERSION
ENV LC_ALL=en_US.UTF-8 \
    LANG=en_US.UTF-8 \
    LANGUAGE=en_US:en \
    TZ=UTC
WORKDIR /app/client

COPY --from=builder /app /app

ENTRYPOINT ["dotnet", "PT.Fibonacci.Client.dll", "3"]