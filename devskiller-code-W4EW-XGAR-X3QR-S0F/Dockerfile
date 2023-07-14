FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Api/Api.csproj", "Api/Api.csproj"]
COPY ["src/AppServices/AppServices.csproj", "AppServices/AppServices.csproj"]
COPY ["src/Model/Model.csproj", "Model/Model.csproj"]
COPY ["src/Repository/Repository.csproj", "Repository/Repository.csproj"]
COPY ["src/CrossCutting/CrossCutting.csproj", "CrossCutting/CrossCutting.csproj"]

RUN dotnet restore "Api/Api.csproj" 
COPY . .
WORKDIR "/src/src/"
RUN dotnet build "Api/Api.csproj" -c Release -o /app/build 

FROM build AS publish
RUN dotnet publish "Api/Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]