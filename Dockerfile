FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY src/Server/CrystaLearn.Server.Web/*.csproj ./Server/
COPY src/Client/CrystaLearn.Client.Web/*.csproj ./Client/Web/
COPY src/Client/CrystaLearn.Client.Core/*.csproj ./Client/Core/
RUN dotnet restore ./Server/CrystaLearn.Server.Web.csproj

COPY . .
RUN dotnet publish ./src/Server/CrystaLearn.Server.Web/CrystaLearn.Server.Web.csproj \
    -c Release \
    -o /app/publish \
    --self-contained true \
    -r linux-x64 \
    /p:UseAppHost=true

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["./CrystaLearn.Server.Web"]