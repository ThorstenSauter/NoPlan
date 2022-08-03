FROM mcr.microsoft.com/dotnet/aspnet:7.0.0-preview.6-alpine3.16-amd64 AS base
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0.100-preview.6-alpine3.16-amd64 AS build
WORKDIR /src
COPY ["src/NoPlan.Api/NoPlan.Api.csproj", "NoPlan.Api/"]
COPY ["src/NoPlan.Infrastructure/NoPlan.Infrastructure.csproj", "NoPlan.Infrastructure/"]
COPY ["src/NoPlan.Contracts/NoPlan.Contracts.csproj", "NoPlan.Contracts/"]
RUN dotnet restore "NoPlan.Api/NoPlan.Api.csproj"
COPY ["src/NoPlan.Api/", "NoPlan.Api/"]
COPY ["src/NoPlan.Infrastructure/", "NoPlan.Infrastructure/"]
COPY ["src/NoPlan.Contracts/", "NoPlan.Contracts/"]
WORKDIR "/src/NoPlan.Api"
RUN dotnet build -c Release -r linux-x64 --no-self-contained "NoPlan.Api.csproj"

FROM build AS publish
RUN dotnet publish -c Release --no-build -r linux-x64 --no-self-contained -o /app/publish "NoPlan.Api.csproj"

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NoPlan.Api.dll"]
