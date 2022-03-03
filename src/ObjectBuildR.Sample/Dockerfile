FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/ObjectBuildR.Sample/ObjectBuildR.Sample.csproj", "ObjectBuildR.Sample/"]
RUN dotnet restore "src/ObjectBuildR.Sample/ObjectBuildR.Sample.csproj"
COPY . .
WORKDIR "/src/ObjectBuildR.Sample"
RUN dotnet build "ObjectBuildR.Sample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ObjectBuildR.Sample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ObjectBuildR.Sample.dll"]
