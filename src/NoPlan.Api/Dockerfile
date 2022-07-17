FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/NoPlan.Api/NoPlan.Api.csproj", "NoPlan.Api/"]
COPY ["src/NoPlan.Api/packages.lock.json", "NoPlan.Api/"]
COPY ["src/NoPlan.Infrastructure/NoPlan.Infrastructure.csproj", "NoPlan.Infrastructure/"]
COPY ["src/NoPlan.Infrastructure/packages.lock.json", "NoPlan.Infrastructure/"]
COPY ["src/NoPlan.Contracts/NoPlan.Contracts.csproj", "NoPlan.Contracts/"]
COPY ["src/NoPlan.Contracts/packages.lock.json", "NoPlan.Contracts/"]
RUN dotnet restore "NoPlan.Api/NoPlan.Api.csproj" --locked-mode
COPY ["src/NoPlan.Api/", "NoPlan.Api/"]
COPY ["src/NoPlan.Infrastructure/", "NoPlan.Infrastructure/"]
COPY ["src/NoPlan.Contracts/", "NoPlan.Contracts/"]
WORKDIR "/src/NoPlan.Api"
RUN dotnet build -c Release --no-restore -o /app/build "NoPlan.Api.csproj"

FROM build AS publish
RUN dotnet publish -c Release --no-build -r linux-x64 --no-self-contained -o /app/publish "NoPlan.Api.csproj"

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NoPlan.Api.dll"]
