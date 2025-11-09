# Production Database Fix - 500 Error on User Registration

## Problem
When attempting to register a user in production, the API returns a 500 error. This is caused by SQLite database not being able to write to the `/app/data/` directory due to permission issues.

## Root Cause
Most hosting platforms (Render, Railway, Fly.io, etc.) have read-only file systems in the `/app` directory. The application was trying to create and write to `/app/data/portfolio.db`, which fails due to insufficient permissions.

## Solution Applied

### 1. Updated DatabaseContext.cs
The database path now uses a writable directory:
- In production, it uses `/tmp` by default (writable on most platforms)
- You can override this by setting the `DATA_DIR` environment variable
- Added better error logging to diagnose database initialization issues

### 2. Key Changes:
```csharp
// Now uses /tmp or DATA_DIR environment variable
var dataDir = Environment.GetEnvironmentVariable("DATA_DIR") ?? "/tmp";
var fullPath = Path.Combine(dataDir, "portfolio.db");
```

## Deployment Steps

### Option 1: Use /tmp directory (Ephemeral - Data lost on restart)
This is suitable for testing, but **data will be lost on every deployment or restart**.

**Add this environment variable to your server:**
```bash
DATA_DIR=/tmp
```

### Option 2: Use Platform-Specific Persistent Storage (RECOMMENDED)

#### For Render.com:
1. Create a disk in your Render dashboard
2. Mount it to a path like `/data`
3. Add environment variable:
```bash
DATA_DIR=/data
```

#### For Railway:
1. Add a volume in your Railway project settings
2. Mount it to `/data`
3. Add environment variable:
```bash
DATA_DIR=/data
```

#### For Fly.io:
1. Create a volume: `fly volumes create portfolio_data --size 1`
2. Mount it in your `fly.toml`:
```toml
[mounts]
  source = "portfolio_data"
  destination = "/data"
```
3. Add environment variable:
```bash
DATA_DIR=/data
```

### Option 3: Alternative Connection String Override
You can also set the connection string directly via environment variable (not implemented yet, but easy to add).

## Current Environment Variables Required

```bash
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:10000
JWT_AUDIENCE=PortfolioAdmin.Client
JWT_ISSUER=PortfolioAdmin.Api
JWT_SECRET_KEY=dc2b2ecdd56e7e93ba973e5e322997fb
DATA_DIR=/tmp  # or your persistent storage path
```

## Testing the Fix

### 1. Check Database Path in Logs
When the application starts, you should see:
```
Using database path: Data Source=/tmp/portfolio.db
Environment: Production
Initializing database...
Database connection opened successfully
Database initialization completed successfully
```

### 2. Test User Registration
```bash
curl -X POST https://your-api.com/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@example.com",
    "password": "YourSecurePassword123!",
    "role": "Admin"
  }'
```

Expected response (success):
```json
{
  "id": 1,
  "email": "admin@example.com",
  "role": "Admin",
  "lastLoginAt": null
}
```

### 3. Verify Database File
SSH into your server and check:
```bash
ls -la /tmp/portfolio.db
# or
ls -la /data/portfolio.db
```

## Important Notes

⚠️ **WARNING: /tmp is ephemeral!**
- Data in `/tmp` is lost on server restart
- Only use `/tmp` for testing
- Use persistent volumes for production

📝 **Database Persistence:**
- Always use platform-specific persistent storage
- Set up regular backups of your database file
- Consider migrating to PostgreSQL for production if scaling

🔍 **Debugging:**
- Check application logs for database initialization messages
- Look for permission errors in the logs
- Verify the DATA_DIR environment variable is set correctly

## Rollback
If you need to rollback, the original code expected the database at:
```
/app/data/portfolio.db
```

## Next Steps
1. Deploy the updated code
2. Set the `DATA_DIR` environment variable
3. Restart your application
4. Test user registration
5. Set up database backups

## Additional Improvements to Consider

### 1. Add Connection String Environment Variable Support
```csharp
// In Program.cs or DatabaseContext.cs
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
if (!string.IsNullOrEmpty(connectionString))
{
    _connectionString = connectionString;
}
```

### 2. Database Backup Script
Create a cron job to backup the database:
```bash
#!/bin/bash
cp /data/portfolio.db /data/backups/portfolio-$(date +%Y%m%d-%H%M%S).db
```

### 3. Migrate to PostgreSQL (Optional)
For better scalability and data persistence, consider migrating to PostgreSQL:
- Most cloud platforms offer managed PostgreSQL
- Better for multi-instance deployments
- More robust than SQLite for production

## Support
If issues persist:
1. Check server logs: `tail -f /var/log/your-app.log`
2. Verify file permissions: `ls -la /tmp` or `ls -la /data`
3. Test database write access: `touch /tmp/test.txt`
4. Check disk space: `df -h`
