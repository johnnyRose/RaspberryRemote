dotnet clean
dotnet build
dotnet publish -c Debug -r linux-arm
rsync --rsh /cygdrive/c/cygwin64/bin/ssh -avzh bin/Debug/netcoreapp2.1/linux-arm/publish pi@raspberrypi.local:~/
