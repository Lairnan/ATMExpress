FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:8080
WORKDIR /app


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["AccessHub/AccessHub.csproj", "AccessHub/"]
COPY ["DatabaseManagement/DatabaseManagement.csproj", "DatabaseManagement/"]
COPY ["CSA/CSA.csproj", "CSA/"]
RUN dotnet restore "AccessHub/AccessHub.csproj"
COPY . .
WORKDIR "/src/AccessHub"
RUN dotnet build "AccessHub.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AccessHub.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "AccessHub.dll"]
