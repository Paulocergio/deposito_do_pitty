FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia arquivos "de entrada" primeiro para aproveitar cache do restore
COPY ["deposito_do_pitty.sln", "./"]
COPY ["deposito_do_pitty/deposito_do_pitty.csproj", "deposito_do_pitty/"]

RUN dotnet restore "deposito_do_pitty/deposito_do_pitty.csproj"

# Copia o resto do repositório
COPY . .

RUN dotnet publish "deposito_do_pitty/deposito_do_pitty.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "deposito_do_pitty.dll"]
