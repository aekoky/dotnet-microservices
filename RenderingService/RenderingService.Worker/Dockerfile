#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
RUN apt update
RUN apt install -y libgdiplus
RUN ln -s /usr/lib/libgdiplus.so /lib/x86_64-linux-gnu/libgdiplus.so
RUN apt-get install -y --no-install-recommends zlib1g fontconfig libfreetype6 libx11-6 libxext6 libxrender1 wget gdebi
RUN wget https://github.com/wkhtmltopdf/wkhtmltopdf/releases/download/0.12.5/wkhtmltox_0.12.5-1.stretch_amd64.deb
RUN gdebi -n wkhtmltox_0.12.5-1.stretch_amd64.deb
RUN ln -s /usr/local/lib/libwkhtmltox.so /usr/lib/libwkhtmltox.so

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["RenderingService/RenderingService.Worker/RenderingService.Worker.csproj", "RenderingService/RenderingService.Worker/"]
COPY ["Core/Formuler.Core/Formuler.Core.csproj", "Core/Formuler.Core/"]
COPY ["RenderingService/RenderingService.Data/RenderingService.Data.csproj", "RenderingService/RenderingService.Data/"]
RUN dotnet restore "RenderingService/RenderingService.Worker/RenderingService.Worker.csproj"
COPY . .
WORKDIR "/src/RenderingService/RenderingService.Worker"
RUN dotnet build "RenderingService.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RenderingService.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RenderingService.Worker.dll"]