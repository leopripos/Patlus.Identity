#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:5.0-buster AS build
WORKDIR /src
COPY ["Patlus.IdentityManagement.EventHandler/Patlus.IdentityManagement.EventHandler.csproj", "Patlus.IdentityManagement.EventHandler/"]
COPY ["Patlus.IdentityManagement.Presentation/Patlus.IdentityManagement.Presentation.csproj", "Patlus.IdentityManagement.Presentation/"]
COPY ["Patlus.IdentityManagement.Cache/Patlus.IdentityManagement.Cache.csproj", "Patlus.IdentityManagement.Cache/"]
COPY ["Patlus.IdentityManagement.UseCase/Patlus.IdentityManagement.UseCase.csproj", "Patlus.IdentityManagement.UseCase/"]
COPY ["Patlus.Common.UseCase/Patlus.Common.UseCase.csproj", "Patlus.Common.UseCase/"]
COPY ["Patlus.Common.UseCase.Abstraction/Patlus.Common.UseCase.Abstraction.csproj", "Patlus.Common.UseCase.Abstraction/"]
COPY ["Patlus.IdentityManagement.UseCase.Abstraction/Patlus.IdentityManagement.UseCase.Abstraction.csproj", "Patlus.IdentityManagement.UseCase.Abstraction/"]
COPY ["Patlus.IdentityManagement.Persistence/Patlus.IdentityManagement.Persistence.csproj", "Patlus.IdentityManagement.Persistence/"]
COPY ["Patlus.IdentityManagement.Presentation.Abstraction/Patlus.IdentityManagement.Presentation.Abstraction.csproj", "Patlus.IdentityManagement.Presentation.Abstraction/"]
COPY ["Patlus.Common.Presentation.Abstraction/Patlus.Common.Presentation.Abstraction.csproj", "Patlus.Common.Presentation.Abstraction/"]
COPY ["Patlus.IdentityManagement.EventDispatcher/Patlus.IdentityManagement.EventDispatcher.csproj", "Patlus.IdentityManagement.EventDispatcher/"]
COPY ["Patlus.Common.Presentation/Patlus.Common.Presentation.csproj", "Patlus.Common.Presentation/"]
RUN dotnet restore "Patlus.IdentityManagement.EventHandler/Patlus.IdentityManagement.EventHandler.csproj"
COPY . .
WORKDIR "/src/Patlus.IdentityManagement.EventHandler"
RUN dotnet build "Patlus.IdentityManagement.EventHandler.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Patlus.IdentityManagement.EventHandler.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Patlus.IdentityManagement.EventHandler.dll"]