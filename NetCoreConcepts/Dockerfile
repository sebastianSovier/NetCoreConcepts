

# Utiliza la imagen oficial de .NET Core 6.0
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NetCoreConcepts/NetCoreConcepts.csproj", "NetCoreConcepts/"]
COPY ["Models/Models.csproj", "Models/"]
COPY ["Negocio/Negocio.csproj", "Negocio/"]
COPY ["Datos/Datos.csproj", "Datos/"]
COPY ["Utilidades/Utilidades.csproj", "Utilidades/"]

RUN dotnet restore "NetCoreConcepts/NetCoreConcepts.csproj"

COPY . .
WORKDIR "/src/NetCoreConcepts"
RUN dotnet build "NetCoreConcepts.csproj" -c Release -o /app/publish

FROM build AS publish
RUN dotnet publish  "NetCoreConcepts.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetCoreConcepts.dll"]