#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["TemplateService/TemplateService.Api/TemplateService.Api.csproj", "TemplateService/TemplateService.Api/"]
COPY ["Core/Formuler.Shared/Formuler.Shared.csproj", "Core/Formuler.Shared/"]
COPY ["Core/Formuler.Core/Formuler.Core.csproj", "Core/Formuler.Core/"]
COPY ["TemplateService/TemplateService.Business/TemplateService.Business.csproj", "TemplateService/TemplateService.Business/"]
COPY ["TemplateService/TemplateService.Data/TemplateService.Data.csproj", "TemplateService/TemplateService.Data/"]
RUN dotnet restore "TemplateService/TemplateService.Api/TemplateService.Api.csproj"
COPY . .
WORKDIR "/src/TemplateService/TemplateService.Api"
RUN dotnet build "TemplateService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TemplateService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TemplateService.Api.dll"]
