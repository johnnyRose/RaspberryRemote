dotnet clean
dotnet build
dotnet publish -c Debug -r linux-arm
scp -r bin/Debug/netcoreapp2.1/linux-arm/publish pi@raspberrypi.local:~/publish
