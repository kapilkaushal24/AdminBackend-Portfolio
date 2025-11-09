#!/bin/bash

# Portfolio Admin API - Render Deployment Script
# This script helps prepare and deploy your API to Render

set -e

echo "🚀 Portfolio Admin API - Render Deployment Helper"
echo "================================================="

# Check if we're in the right directory
if [ ! -f "PortfolioAdmin.Api.csproj" ]; then
    echo "❌ Error: Please run this script from the PortfolioAdmin.Api directory"
    exit 1
fi

echo "✅ Found PortfolioAdmin.Api.csproj"

# Check if Docker is installed
if ! command -v docker &> /dev/null; then
    echo "⚠️  Docker is not installed. Please install Docker to test the build locally."
else
    echo "✅ Docker is available"
    
    # Ask if user wants to test Docker build
    read -p "🔨 Do you want to test the Docker build locally? (y/N): " test_build
    if [[ $test_build =~ ^[Yy]$ ]]; then
        echo "🔨 Building Docker image..."
        docker build -t portfolio-admin-api-test .
        
        if [ $? -eq 0 ]; then
            echo "✅ Docker build successful!"
            
            read -p "🧪 Do you want to run the container locally for testing? (y/N): " run_test
            if [[ $run_test =~ ^[Yy]$ ]]; then
                echo "🧪 Running container on port 8080..."
                echo "📍 API will be available at: http://localhost:8080"
                echo "📍 Swagger UI will be available at: http://localhost:8080/swagger"
                echo "📍 Health check: http://localhost:8080/api/health"
                echo ""
                echo "Press Ctrl+C to stop the container"
                docker run -p 8080:8080 \
                    -e ASPNETCORE_ENVIRONMENT=Development \
                    -e JWT_SECRET_KEY=your-super-secret-jwt-key-for-portfolio-admin-system-2024 \
                    -e JWT_ISSUER=PortfolioAdmin.Api \
                    -e JWT_AUDIENCE=PortfolioAdmin.Client \
                    portfolio-admin-api-test
            fi
        else
            echo "❌ Docker build failed. Please check the errors above."
            exit 1
        fi
    fi
fi

echo ""
echo "📋 Pre-deployment Checklist:"
echo "=============================="

# Check for required files
files=("Dockerfile" ".dockerignore" ".github/workflows/deploy.yml" "render.yaml" "start.sh" "appsettings.Production.json")
for file in "${files[@]}"; do
    if [ -f "$file" ]; then
        echo "✅ $file"
    else
        echo "❌ $file (missing)"
    fi
done

echo ""
echo "🔧 Next Steps:"
echo "=============="
echo "1. 📤 Push your code to GitHub"  
echo "2. 🌐 Go to https://dashboard.render.com/"
echo "3. ➕ Create a new Web Service"
echo "4. 🔗 Connect your GitHub repository"
echo "5. ⚙️  Configure the following settings:"
echo ""
echo "   📋 Basic Configuration:"
echo "   - Environment: Docker"
echo "   - Build Command: docker build -t portfolio-admin-api ."
echo "   - Start Command: ./start.sh"
echo "   - Port: 10000"
echo ""
echo "   🔐 Environment Variables:"
echo "   - ASPNETCORE_ENVIRONMENT=Production"
echo "   - ASPNETCORE_URLS=http://+:10000"
echo "   - JWT_SECRET_KEY=[Generate a secure 32+ character key]"
echo "   - JWT_ISSUER=PortfolioAdmin.Api"
echo "   - JWT_AUDIENCE=PortfolioAdmin.Client"
echo ""
echo "   ❤️  Health Check:"
echo "   - Health Check Path: /api/health"
echo ""
echo "6. 🔑 Set up GitHub Secrets for CI/CD:"
echo "   - RENDER_API_KEY (from Render Account Settings)"
echo "   - RENDER_SERVICE_ID (from your service URL)"
echo ""
echo "📖 For detailed instructions, see DEPLOYMENT.md"
echo ""
echo "🎉 Good luck with your deployment!"