# How to

To launch the application inside the docker container, run the following command

```shell
docker-compose -f docker-compose.yml up
```

Or you can run applications in a more familiar way
```powershell
dotnet build ./src -c Release
```

#### Api:

```powershell
cd ./src/PT.Fibonacci.Api/bin/Release/netcoreapp3.1/
PT.Fibonacci.Api.exe
```

#### Client:

```powershell
cd ./src/PT.Fibonacci.Client/bin/Release/netcoreapp3.1
PT.Fibonacci.Client.exe 3
```

> The client accepts as an argument any number that indicates how many parallel calculations will be started

#### Nats

```shell
docker run -p 8222:8222 -p 4222:4222 -p 6222:6222 -d nats
```

#### Configuration

Application settings are performed in the file **application.json**

