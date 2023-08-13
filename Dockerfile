FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
WORKDIR /app
COPY [".editorconfig", "Directory.Build.props", "Directory.Packages.props", "nuget.config", "./"]
COPY ["src/Directory.Build.props", "src/"]
COPY ["src/NoPlan.Api/NoPlan.Api.csproj", "src/NoPlan.Api/"]
COPY ["src/NoPlan.Contracts/NoPlan.Contracts.csproj", "src/NoPlan.Contracts/"]
COPY ["src/NoPlan.Infrastructure/NoPlan.Infrastructure.csproj", "src/NoPlan.Infrastructure/"]
RUN dotnet restore -r linux-x64 "src/NoPlan.Api/NoPlan.Api.csproj"
COPY ["src/NoPlan.Api/", "src/NoPlan.Api/"]
COPY ["src/NoPlan.Contracts/", "src/NoPlan.Contracts/"]
COPY ["src/NoPlan.Infrastructure/", "src/NoPlan.Infrastructure/"]

# Copy the git repository data for Source Link
COPY [".git/", ".git/"]
WORKDIR /app/src/NoPlan.Api
RUN dotnet build --no-restore -c Release -r linux-x64 --no-self-contained "NoPlan.Api.csproj"

FROM build AS publish
RUN dotnet publish --no-build -r linux-x64 --no-self-contained -o /app/publish "NoPlan.Api.csproj"

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NoPlan.Api.dll"]
