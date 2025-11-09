# Portfolio Admin API - Render Deployment Guide

## 🚀 Deployment Overview

This guide will help you deploy your .NET 9.0 Portfolio Admin API to Render with automated CI/CD using GitHub Actions.

## 📋 Prerequisites

- GitHub account
- Render account (free tier available)
- Your code pushed to a GitHub repository

## 🔧 Setup Steps

### 1. GitHub Repository Setup

Ensure your code is pushed to GitHub and includes all the deployment files:
- `Dockerfile`
- `.dockerignore`
- `.github/workflows/deploy.yml`
- `render.yaml`
- `start.sh`
- `appsettings.Production.json`

### 2. Render Service Creation

1. **Go to Render Dashboard**: https://dashboard.render.com/
2. **Create New Web Service**:
   - Connect your GitHub repository
   - Choose "Docker" as the environment
   - Set the following configuration:

#### Basic Settings:
- **Name**: `portfolio-admin-api`
- **Region**: Choose closest to your users
- **Branch**: `main` (or `master`)
- **Build Command**: `docker build -t portfolio-admin-api .`
- **Start Command**: `./start.sh`

#### Environment Variables:
Set these in Render Dashboard under "Environment":

| Variable | Value | Secret |
|----------|-------|--------|
| `ASPNETCORE_ENVIRONMENT` | `Production` | No |
| `ASPNETCORE_URLS` | `http://+:10000` | No |
| `JWT_SECRET_KEY` | `your-super-secret-jwt-key-minimum-32-characters-long` | Yes |
| `JWT_ISSUER` | `PortfolioAdmin.Api` | No |
| `JWT_AUDIENCE` | `PortfolioAdmin.Client` | No |

#### Advanced Settings:
- **Health Check Path**: `/api/health`
- **Auto-Deploy**: `Yes`

### 3. GitHub Secrets Setup

For the CI/CD pipeline, add these secrets to your GitHub repository:

1. Go to your repository on GitHub
2. Navigate to **Settings** > **Secrets and variables** > **Actions**
3. Add the following secrets:

| Secret Name | Value | How to Get |
|------------|-------|------------|
| `RENDER_API_KEY` | Your Render API key | Render Dashboard > Account Settings > API Keys |
| `RENDER_SERVICE_ID` | Your service ID | From the Render service URL or API |

### 4. Database Configuration

The application uses SQLite which will be stored in `/app/data/portfolio.db` in the container. For production, consider:

- **Free Option**: Keep SQLite (data persists between deployments)
- **Recommended**: Upgrade to PostgreSQL using Render's database service

#### PostgreSQL Setup (Recommended):
1. Create a PostgreSQL database on Render
2. Update connection string in `appsettings.Production.json`
3. Install EF Core PostgreSQL package: `Microsoft.EntityFrameworkCore.Npgsql`

### 5. File Upload Configuration

Current setup stores files in container filesystem. For production:

- **Free Option**: Files stored in `/app/wwwroot/uploads` (will be lost on redeploy)
- **Recommended**: Use cloud storage (AWS S3, Cloudinary, etc.)

## 🔄 CI/CD Pipeline

The GitHub Actions workflow will:

1. **On Pull Request**: Run tests and build validation
2. **On Push to Main**: 
   - Run tests
   - Build application
   - Deploy to Render automatically

## 🌐 Custom Domain (Optional)

1. In Render Dashboard, go to your service
2. Navigate to **Settings** > **Custom Domains**
3. Add your domain and configure DNS

## 📊 Monitoring

### Health Checks
- Basic: `https://your-app.onrender.com/api/health`
- Detailed: `https://your-app.onrender.com/api/health/detailed`

### API Documentation
- Swagger UI: `https://your-app.onrender.com/swagger`

### Logs
- View logs in Render Dashboard under your service

## 🛡️ Security Considerations

1. **Environment Variables**: All secrets are properly configured as environment variables
2. **CORS**: Update the allowed origins in `Program.cs` with your actual frontend URLs
3. **HTTPS**: Render provides SSL certificates automatically
4. **JWT**: Use a strong secret key (minimum 32 characters)

## 🔧 Troubleshooting

### Common Issues:

1. **Build Failures**:
   - Check Docker logs in Render
   - Verify all dependencies in `.csproj`

2. **Database Issues**:
   - Ensure database directory exists: `/app/data`
   - Check file permissions

3. **CORS Issues**:
   - Update allowed origins in `Program.cs`
   - Verify frontend URLs

4. **Environment Variables**:
   - Ensure all required variables are set in Render
   - Check for typos in variable names

### Useful Commands:

```bash
# Test Docker build locally
docker build -t portfolio-admin-api .
docker run -p 8080:8080 portfolio-admin-api

# Check service logs
curl -H "Authorization: Bearer $RENDER_API_KEY" \
  "https://api.render.com/v1/services/$RENDER_SERVICE_ID/logs"
```

## 📈 Scaling Options

### Free Tier Limitations:
- Sleeps after 15 minutes of inactivity
- 512MB RAM
- Limited bandwidth

### Paid Tier Benefits:
- No sleeping
- More resources
- Custom domains
- Priority support

## 🚀 Next Steps

1. Deploy and test your API
2. Update frontend CORS origins
3. Set up monitoring and alerts
4. Consider database upgrade
5. Implement file upload to cloud storage
6. Set up custom domain

## 📞 Support

- **Render Docs**: https://render.com/docs
- **GitHub Actions**: https://docs.github.com/actions
- **.NET on Docker**: https://docs.microsoft.com/dotnet/core/docker/

---

Your Portfolio Admin API should now be successfully deployed on Render with automated CI/CD! 🎉