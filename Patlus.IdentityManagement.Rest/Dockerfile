#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:5.0-buster AS build
WORKDIR /src
COPY ["Patlus.IdentityManagement.Rest/Patlus.IdentityManagement.Rest.csproj", "Patlus.IdentityManagement.Rest/"]
COPY ["Patlus.IdentityManagement.UseCase/Patlus.IdentityManagement.UseCase.csproj", "Patlus.IdentityManagement.UseCase/"]
COPY ["Patlus.Common.UseCase/Patlus.Common.UseCase.csproj", "Patlus.Common.UseCase/"]
COPY ["Patlus.IdentityManagement.Persistence/Patlus.IdentityManagement.Persistence.csproj", "Patlus.IdentityManagement.Persistence/"]
RUN dotnet restore "Patlus.IdentityManagement.Rest/Patlus.IdentityManagement.Rest.csproj"
COPY . .
WORKDIR "/src/Patlus.IdentityManagement.Rest"
RUN dotnet build "Patlus.IdentityManagement.Rest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Patlus.IdentityManagement.Rest.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Patlus.IdentityManagement.Rest.dll"]