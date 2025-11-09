# Portfolio Admin API - Render Deployment

## 🚀 Quick Start

Your .NET Portfolio Admin API is now ready for deployment to Render with automated CI/CD!

## 📁 What's Been Created

### Deployment Files
- **`Dockerfile`** - Multi-stage Docker build for production
- **`.dockerignore`** - Optimized Docker context
- **`start.sh`** - Production startup script
- **`appsettings.Production.json`** - Production configuration

### CI/CD Pipeline
- **`.github/workflows/deploy.yml`** - GitHub Actions workflow
- **`render.yaml`** - Render service configuration

### Monitoring & Health
- **`Controllers/HealthController.cs`** - Health check endpoints
- **`Tests/HealthControllerTests.cs`** - Basic integration tests

### Deployment Scripts
- **`deploy.ps1`** - Windows PowerShell deployment helper
- **`deploy.sh`** - Linux/macOS deployment helper
- **`DEPLOYMENT.md`** - Comprehensive deployment guide

## 🎯 Deployment Steps

### 1. Push to GitHub
```bash
git add .
git commit -m "Add Render deployment configuration"
git push origin main
```

### 2. Create Render Service
1. Go to [Render Dashboard](https://dashboard.render.com/)
2. Click "New" → "Web Service"
3. Connect your GitHub repository
4. Configure as Docker service

### 3. Environment Variables
Set these in Render Dashboard:
```
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:10000
JWT_SECRET_KEY=your-secure-32-character-key-here
JWT_ISSUER=PortfolioAdmin.Api
JWT_AUDIENCE=PortfolioAdmin.Client
```

### 4. GitHub Secrets (for CI/CD)
Add to your repository secrets:
- `RENDER_API_KEY` - From Render account settings
- `RENDER_SERVICE_ID` - From your service URL

## 🔗 Service URLs
Once deployed, your API will be available at:
- **API Base**: `https://your-service.onrender.com`
- **Swagger UI**: `https://your-service.onrender.com/swagger`
- **Health Check**: `https://your-service.onrender.com/api/health`

## 🛠️ Testing Locally
Run the deployment script:
```powershell
# Windows
.\deploy.ps1

# Linux/macOS
chmod +x deploy.sh
./deploy.sh
```

## 📖 Documentation
See `DEPLOYMENT.md` for detailed instructions and troubleshooting.

## 🔄 CI/CD Workflow
- **Pull Request**: Runs tests and build validation
- **Push to Main**: Automatically deploys to Render

---

**Ready to deploy?** Follow the steps above and your API will be live in minutes! 🎉