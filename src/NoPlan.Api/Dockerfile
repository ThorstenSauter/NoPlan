FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/NoPlan.Api/NoPlan.Api.csproj", "NoPlan.Api/"]
COPY ["src/NoPlan.Infrastructure/NoPlan.Infrastructure.csproj", "NoPlan.Infrastructure/"]
COPY ["src/NoPlan.Contracts/NoPlan.Contracts.csproj", "NoPlan.Contracts/"]
RUN dotnet restore "NoPlan.Api/NoPlan.Api.csproj"
COPY ["src/NoPlan.Api/", "NoPlan.Api/"]
COPY ["src/NoPlan.Infrastructure/", "NoPlan.Infrastructure/"]
COPY ["src/NoPlan.Contracts/", "NoPlan.Contracts/"]
WORKDIR "/src/NoPlan.Api"
RUN dotnet build "NoPlan.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NoPlan.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NoPlan.Api.dll"]
