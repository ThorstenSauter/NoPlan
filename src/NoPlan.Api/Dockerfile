FROM golang:1.20.0 as chisel

RUN git clone --depth 1 -b main https://github.com/canonical/chisel /opt/chisel
WORKDIR /opt/chisel
RUN go build ./cmd/chisel


FROM mcr.microsoft.com/dotnet/sdk:7.0-jammy AS build

RUN apt-get update \
    && apt-get install -y fdupes \
    && rm -rf /var/lib/apt/lists/*

COPY --from=chisel /opt/chisel/chisel /usr/bin/
COPY --from=mcr.microsoft.com/dotnet/nightly/runtime:7.0-jammy-chiseled / /runtime-ref

RUN mkdir /rootfs \
    && chisel cut --release "ubuntu-22.04" --root /rootfs \
        libicu70_libs \
    \
    # Remove duplicates from rootfs that exist in runtime-ref
    && fdupes /runtime-ref /rootfs -rdpN \
    \
    # Delete duplicate symlinks
    # Function to find and format symlinks w/o including root dir (format: /path/to/symlink /path/to/target)
    && getsymlinks() { find $1 -type l -printf '%p %l\n' | sed -n "s/^\\$1\\(.*\\)/\\1/p"; } \
    # Combine set of symlinks between rootfs and runtime-ref
    && (getsymlinks "/rootfs"; getsymlinks "/runtime-ref") \
        # Sort them
        | sort \
        # Find the duplicates
        | uniq -d \
        # Extract just the path to the symlink
        | cut -d' ' -f1 \
        # Prepend the rootfs directory to the paths
        | sed -e 's/^/\/rootfs/' \
        # Delete the files
        | xargs rm \
    \
    # Delete empty directories
    && find /rootfs -type d -empty -delete

WORKDIR /src
COPY ["src/NoPlan.Api/NoPlan.Api.csproj", "NoPlan.Api/"]
COPY ["src/NoPlan.Infrastructure/NoPlan.Infrastructure.csproj", "NoPlan.Infrastructure/"]
COPY ["src/NoPlan.Contracts/NoPlan.Contracts.csproj", "NoPlan.Contracts/"]
COPY ["./Directory.Packages.props", "./"]
RUN dotnet restore "NoPlan.Api/NoPlan.Api.csproj"

COPY ["src/NoPlan.Api/", "NoPlan.Api/"]
COPY ["src/NoPlan.Infrastructure/", "NoPlan.Infrastructure/"]
COPY ["src/NoPlan.Contracts/", "NoPlan.Contracts/"]
WORKDIR "/src/NoPlan.Api"
RUN dotnet build -c Release -r linux-x64 --no-self-contained "NoPlan.Api.csproj"
RUN dotnet publish -c Release --no-build -r linux-x64 --no-self-contained -o /app/publish "NoPlan.Api.csproj"

FROM mcr.microsoft.com/dotnet/nightly/aspnet:7.0-jammy-chiseled AS final
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
COPY --from=build /rootfs /
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "NoPlan.Api.dll"]
