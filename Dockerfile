FROM mcr.microsoft.com/dotnet/runtime-deps:8.0-jammy-chiseled-extra AS base
WORKDIR /app
EXPOSE 8080

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG TARGETARCH
WORKDIR /app
COPY [".editorconfig", "Directory.Build.props", "Directory.Packages.props", "nuget.config", "./"]
COPY ["src/Directory.Build.props", "src/"]
COPY ["src/NoPlan.Api/NoPlan.Api.csproj", "src/NoPlan.Api/"]
COPY ["src/NoPlan.Contracts/NoPlan.Contracts.csproj", "src/NoPlan.Contracts/"]
COPY ["src/NoPlan.Infrastructure/NoPlan.Infrastructure.csproj", "src/NoPlan.Infrastructure/"]
RUN dotnet restore -a $TARGETARCH "src/NoPlan.Api/NoPlan.Api.csproj"
COPY ["src/NoPlan.Api/", "src/NoPlan.Api/"]
COPY ["src/NoPlan.Contracts/", "src/NoPlan.Contracts/"]
COPY ["src/NoPlan.Infrastructure/", "src/NoPlan.Infrastructure/"]

# Copy the git repository data for Source Link
COPY [".git/", ".git/"]
WORKDIR /app/src/NoPlan.Api
RUN dotnet build --no-restore -c Release -a $TARGETARCH --sc "NoPlan.Api.csproj"

FROM build AS publish
RUN dotnet publish --no-build -a $TARGETARCH --sc -o /app/publish "NoPlan.Api.csproj"

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["./NoPlan.Api"]
