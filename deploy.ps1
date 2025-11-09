# Portfolio Admin API - Render Deployment Script (PowerShell)
# This script helps prepare and deploy your API to Render

Write-Host "Portfolio Admin API - Render Deployment Helper" -ForegroundColor Green
Write-Host "=================================================" -ForegroundColor Green

# Check if we're in the right directory
if (-not (Test-Path "PortfolioAdmin.Api.csproj")) {
    Write-Host "Error: Please run this script from the PortfolioAdmin.Api directory" -ForegroundColor Red
    exit 1
}

Write-Host "Found PortfolioAdmin.Api.csproj" -ForegroundColor Green

# Check if Docker is installed
try {
    docker --version | Out-Null
    Write-Host "Docker is available" -ForegroundColor Green
    
    # Ask if user wants to test Docker build
    $testBuild = Read-Host "Do you want to test the Docker build locally? (y/N)"
    if ($testBuild -eq "y" -or $testBuild -eq "Y") {
        Write-Host "Building Docker image..." -ForegroundColor Yellow
        docker build -t portfolio-admin-api-test .
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "Docker build successful!" -ForegroundColor Green
            
            $runTest = Read-Host "Do you want to run the container locally for testing? (y/N)"
            if ($runTest -eq "y" -or $runTest -eq "Y") {
                Write-Host "Running container on port 8080..." -ForegroundColor Yellow
                Write-Host "API will be available at: http://localhost:8080" -ForegroundColor Cyan
                Write-Host "Swagger UI will be available at: http://localhost:8080/swagger" -ForegroundColor Cyan
                Write-Host "Health check: http://localhost:8080/api/health" -ForegroundColor Cyan
                Write-Host ""
                Write-Host "Press Ctrl+C to stop the container" -ForegroundColor Yellow
                
                docker run -p 8080:8080 -e ASPNETCORE_ENVIRONMENT=Development -e JWT_SECRET_KEY=your-super-secret-jwt-key-for-portfolio-admin-system-2024 -e JWT_ISSUER=PortfolioAdmin.Api -e JWT_AUDIENCE=PortfolioAdmin.Client portfolio-admin-api-test
            }
        } else {
            Write-Host "Docker build failed. Please check the errors above." -ForegroundColor Red
            exit 1
        }
    }
} catch {
    Write-Host "Docker is not installed. Please install Docker to test the build locally." -ForegroundColor Yellow
}

Write-Host ""
Write-Host "Pre-deployment Checklist:" -ForegroundColor Blue
Write-Host "==============================" -ForegroundColor Blue

# Check for required files
$files = @("Dockerfile", ".dockerignore", ".github\workflows\deploy.yml", "render.yaml", "start.sh", "appsettings.Production.json")
foreach ($file in $files) {
    if (Test-Path $file) {
        Write-Host "$file" -ForegroundColor Green
    } else {
        Write-Host "$file (missing)" -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "Next Steps:" -ForegroundColor Blue
Write-Host "==============" -ForegroundColor Blue
Write-Host "1. Push your code to GitHub"  
Write-Host "2. Go to https://dashboard.render.com/"
Write-Host "3. Create a new Web Service"
Write-Host "4. Connect your GitHub repository"
Write-Host "5. Configure the following settings:"
Write-Host ""
Write-Host "   Basic Configuration:" -ForegroundColor Cyan
Write-Host "   - Environment: Docker"
Write-Host "   - Build Command: docker build -t portfolio-admin-api ."
Write-Host "   - Start Command: ./start.sh"
Write-Host "   - Port: 10000"
Write-Host ""
Write-Host "   Environment Variables:" -ForegroundColor Cyan
Write-Host "   - ASPNETCORE_ENVIRONMENT=Production"
Write-Host "   - ASPNETCORE_URLS=http://+:10000"
Write-Host "   - JWT_SECRET_KEY=[Generate a secure 32+ character key]"
Write-Host "   - JWT_ISSUER=PortfolioAdmin.Api"
Write-Host "   - JWT_AUDIENCE=PortfolioAdmin.Client"
Write-Host ""
Write-Host "   Health Check:" -ForegroundColor Cyan
Write-Host "   - Health Check Path: /api/health"
Write-Host ""
Write-Host "6. Set up GitHub Secrets for CI/CD:"
Write-Host "   - RENDER_API_KEY (from Render Account Settings)"
Write-Host "   - RENDER_SERVICE_ID (from your service URL)"
Write-Host ""
Write-Host "For detailed instructions, see DEPLOYMENT.md" -ForegroundColor Yellow
Write-Host ""
Write-Host "Good luck with your deployment!" -ForegroundColor Green