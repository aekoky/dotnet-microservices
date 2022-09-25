#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["FileService/FileService.API/FileService.API.csproj", "FileService/FileService.API/"]
COPY ["FileService/FileService.Business/FileService.Business.csproj", "FileService/FileService.Business/"]
COPY ["Core/Formuler.Shared/Formuler.Shared.csproj", "Core/Formuler.Shared/"]
COPY ["Core/Formuler.Core/Formuler.Core.csproj", "Core/Formuler.Core/"]
RUN dotnet restore "FileService/FileService.API/FileService.API.csproj"
COPY . .
WORKDIR "/src/FileService/FileService.API"
RUN dotnet build "FileService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FileService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FileService.API.dll"]
