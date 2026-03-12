# 1. Etapa de compilaci�n (SDK)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copiamos el archivo del proyecto para restaurar paquetes
COPY *.csproj ./
RUN dotnet restore

# Copiamos todo el c�digo y compilamos
COPY . ./
RUN dotnet publish -c Release -o out

# 2. Etapa de ejecuci�n (Runtime) - Mucho m�s ligera
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

# Traemos solo lo necesario de la etapa anterior
COPY --from=build /app/out .

# OJO! Cambia 'TaskFlow.Api.dll' por el nombre real de tu archivo si es distinto
ENTRYPOINT ["dotnet", "TaskFlow.Api.dll"]