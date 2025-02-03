# Use the official .NET 8.0 SDK image as the build environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

# Use the .NET 8.0 SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the server project and restore dependencies
COPY ["server/server.csproj", "server/"]
RUN dotnet restore "server/server.csproj"

# Copy the remaining files and build the project
COPY . .
WORKDIR "/src/server"
RUN dotnet build "server.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "server.csproj" -c Release -o /app/publish

# Final image with only the published app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "server.dll"]
