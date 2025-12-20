FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "deposito_do_pitty/deposito_do_pitty.csproj"
RUN dotnet publish "deposito_do_pitty/deposito_do_pitty.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Se o seu projeto for net9.0, mantenha runtime 9.0 (recomendado)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet","deposito_do_pitty.dll"]
