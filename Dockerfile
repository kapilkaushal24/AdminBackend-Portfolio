# Use the official .NET 9.0 runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 10000

# Use the official .NET 9.0 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY ["PortfolioAdmin.Api.csproj", "."]
RUN dotnet restore "./PortfolioAdmin.Api.csproj"

# Copy the rest of the application code
COPY . .
WORKDIR "/src/."

# Build the application
RUN dotnet build "PortfolioAdmin.Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "PortfolioAdmin.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage/image
FROM base AS final
WORKDIR /app

# Create directories for uploads and data
RUN mkdir -p wwwroot/uploads/general wwwroot/uploads/profile wwwroot/uploads/projects wwwroot/uploads/technologies data

# Copy the published application
COPY --from=publish /app/publish .

# Copy and set permissions for start script
COPY start.sh ./
RUN chmod +x start.sh

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:10000

# Create a non-root user and set permissions
RUN groupadd -r appgroup && useradd -r -g appgroup appuser
RUN chown -R appuser:appgroup /app
USER appuser

ENTRYPOINT ["./start.sh"]