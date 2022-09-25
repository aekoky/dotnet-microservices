#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["UserService/UserService.Api/UserService.Api.csproj", "UserService/UserService.Api/"]
COPY ["Core/Formuler.Core/Formuler.Core.csproj", "Core/Formuler.Core/"]
COPY ["UserService/UserService.Business/UserService.Business.csproj", "UserService/UserService.Business/"]
COPY ["Core/Formuler.Shared/Formuler.Shared.csproj", "Core/Formuler.Shared/"]
COPY ["UserService/UserService.Data/UserService.Data.csproj", "UserService/UserService.Data/"]
RUN dotnet restore "UserService/UserService.Api/UserService.Api.csproj"
COPY . .
WORKDIR "/src/UserService/UserService.Api"
RUN dotnet build "UserService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserService.Api.dll"]
