FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY publish ./
ENTRYPOINT ["dotnet", "CrystallineSociety.Server.dll"]
