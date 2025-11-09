#!/bin/bash

# Create necessary directories
mkdir -p /app/data
mkdir -p /app/wwwroot/uploads/general
mkdir -p /app/wwwroot/uploads/profile  
mkdir -p /app/wwwroot/uploads/projects
mkdir -p /app/wwwroot/uploads/technologies

# Set proper permissions
chmod -R 755 /app/wwwroot/uploads
chmod -R 755 /app/data

# Start the application
exec dotnet PortfolioAdmin.Api.dll