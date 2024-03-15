#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PM.API/PM.API.csproj", "PM.API/"]
COPY ["PM.DB/PM.DB.csproj", "PM.DB/"]
COPY ["PM.Core/PM.Core.csproj", "PM.Core/"]
RUN dotnet restore "PM.API/PM.API.csproj"
COPY . .
WORKDIR "/src/PM.API"
RUN dotnet build "PM.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PM.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://*:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENV TZ=Asia/Taipei
ENTRYPOINT ["dotnet", "PM.API.dll"]
