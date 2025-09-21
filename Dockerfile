FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /cslearn
COPY publish ./
ENTRYPOINT ["dotnet", "CrystaLearn.Server.Web.dll"]
