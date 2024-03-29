# App
`Attention: his application is a test task, so you should not use it in product development`

Application for calculating fibonacci number using distributed interaction via message queue

## How to

To launch the application inside the docker container, run the following command

```console
docker-compose -f docker-compose.yml up
```

Or you can run applications in a more familiar way
```console
dotnet build ./src -c Release
```

#### Api:

```console
cd ./src/PT.Fibonacci.Api/bin/Release/netcoreapp3.1/
dotnet PT.Fibonacci.Api.dll
```

#### Client:

```console
cd ./src/PT.Fibonacci.Client/bin/Release/netcoreapp3.1
dotnet PT.Fibonacci.Client.dll 3
```

> The client accepts as an argument any number that indicates how many parallel calculations will be started

#### Nats

```console
docker run -p 8222:8222 -p 4222:4222 -p 6222:6222 -d nats
```

#### Configuration

Application settings are performed in the file **application.json**

