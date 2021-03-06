FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src

COPY "drDotnet-ServicesAndWebApps.sln" "drDotnet-ServicesAndWebApps.sln"

COPY "Services/Contact/Contact.API/Contact.API.csproj" "Services/Contact/Contact.API/Contact.API.csproj"
COPY "Services/Identity/Identity.API/Identity.API.csproj" "Services/Identity/Identity.API/Identity.API.csproj"
COPY "Services/SignalrHub/SignalrHub.csproj" "Services/SignalrHub/SignalrHub.csproj"
COPY "Web/ReactSPA/drDotnet.ReactSPA.csproj" "Web/ReactSPA/drDotnet.ReactSPA.csproj"

RUN dotnet restore "drDotnet-ServicesAndWebApps.sln"

COPY . .

WORKDIR /src/Services/SignalrHub
RUN dotnet publish -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SignalrHub.dll"]