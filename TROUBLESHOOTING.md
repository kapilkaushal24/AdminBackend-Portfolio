# Render Deployment Troubleshooting

## 🔍 Common Issues and Solutions

### Issue: 404 Page Not Found

**Possible Causes:**
1. Service not fully started
2. Wrong health check endpoint
3. HTTPS redirection issues
4. Port configuration problems

**Solutions Applied:**
- ✅ Added root endpoint redirect to Swagger: `https://your-app.onrender.com/` → `/swagger`
- ✅ Fixed health check path to `/api/health`
- ✅ Disabled HTTPS redirection in production (Render handles this)
- ✅ Configured proper Swagger UI for production

### URLs to Test:

1. **Root**: `https://backend-portfolio-lpjj.onrender.com/`
   - Should redirect to Swagger UI

2. **Swagger UI**: `https://backend-portfolio-lpjj.onrender.com/swagger`
   - Interactive API documentation

3. **Health Check**: `https://backend-portfolio-lpjj.onrender.com/api/health`
   - Should return JSON with "healthy" status

4. **API Base**: `https://backend-portfolio-lpjj.onrender.com/api/`
   - Base path for all API endpoints

### Environment Variables to Verify in Render:

```
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:10000
JWT_SECRET_KEY=[your-secret-key]
JWT_ISSUER=PortfolioAdmin.Api
JWT_AUDIENCE=PortfolioAdmin.Client
```

### Check Render Logs:

1. Go to Render Dashboard
2. Select your service
3. Click "Logs" tab
4. Look for:
   - ✅ "Application started"
   - ✅ "Now listening on: http://[::]:10000"
   - ❌ Any error messages

### If Still Having Issues:

1. **Restart the service** in Render Dashboard
2. **Check build logs** for any errors
3. **Verify environment variables** are set correctly
4. **Test health endpoint first**: `/api/health`

## 🚀 Expected Behavior After Fix:

- Root URL redirects to Swagger UI
- Swagger UI shows all API endpoints
- Health check returns success status
- All API endpoints are accessible