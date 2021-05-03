#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
RUN curl -sL https://deb.nodesource.com/setup_10.x |  bash -
RUN apt-get install -y nodejs

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
RUN curl -sL https://deb.nodesource.com/setup_10.x |  bash -
RUN apt-get install -y nodejs
WORKDIR /src

COPY "drDotnet-ServicesAndWebApps.sln" "drDotnet-ServicesAndWebApps.sln"

COPY "Services/Contact/Contact.API/Contact.API.csproj" "Services/Contact/Contact.API/Contact.API.csproj"
COPY "Services/Identity/Identity.API/Identity.API.csproj" "Services/Identity/Identity.API/Identity.API.csproj"
COPY "Services/SignalrHub/SignalrHub.csproj" "Services/SignalrHub/SignalrHub.csproj"
COPY "Web/ReactSPA/drDotnet.ReactSPA.csproj" "Web/ReactSPA/drDotnet.ReactSPA.csproj"

RUN dotnet restore "drDotnet-ServicesAndWebApps.sln"
COPY . .
WORKDIR "/src/Web/ReactSPA"

FROM build AS publish
RUN dotnet publish "drDotnet.ReactSPA.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "drDotnet.ReactSPA.dll"]