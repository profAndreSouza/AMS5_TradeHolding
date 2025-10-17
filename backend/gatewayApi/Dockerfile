# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia o arquivo de projeto e restaura dependências
COPY *.csproj ./
RUN dotnet restore

# Copia o restante do código
COPY . .

# Publica a aplicação
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copia o resultado do build
COPY --from=build /app/publish .

# Expõe a porta da aplicação
EXPOSE 8080

# Define a URL de execução do ASP.NET
ENV ASPNETCORE_URLS=http://+:8080

# Ponto de entrada
ENTRYPOINT ["dotnet", "gatewayApi.dll"]
