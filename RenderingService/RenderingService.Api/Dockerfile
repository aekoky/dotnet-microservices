#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["RenderingService/RenderingService.Api/RenderingService.Api.csproj", "RenderingService/RenderingService.Api/"]
COPY ["Core/Formuler.Core/Formuler.Core.csproj", "Core/Formuler.Core/"]
COPY ["RenderingService/RenderingService.Data/RenderingService.Data.csproj", "RenderingService/RenderingService.Data/"]
COPY ["RenderingService/RenderingService.Business/RenderingService.Business.csproj", "RenderingService/RenderingService.Business/"]
COPY ["Core/Formuler.Shared/Formuler.Shared.csproj", "Core/Formuler.Shared/"]
RUN dotnet restore "RenderingService/RenderingService.Api/RenderingService.Api.csproj"
COPY . .
WORKDIR "/src/RenderingService/RenderingService.Api"
RUN dotnet build "RenderingService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RenderingService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RenderingService.Api.dll"]
