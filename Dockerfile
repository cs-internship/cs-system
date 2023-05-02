FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY publish ./
ENTRYPOINT ["dotnet", "CrystallineSociety.Server.Api.dll"]
