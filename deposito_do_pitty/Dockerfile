FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["deposito_do_pitty.csproj", "./"]
RUN dotnet restore "./deposito_do_pitty.csproj"

COPY . .
RUN dotnet publish "./deposito_do_pitty.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "deposito_do_pitty.dll"]
