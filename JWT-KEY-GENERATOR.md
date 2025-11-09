# JWT Secret Key Generator

## Generate a Secure JWT Secret Key

Use one of these methods to generate a secure 256-bit (32+ character) JWT secret key:

### Method 1: PowerShell (Windows)
```powershell
# Generate a 64-character random string
-join ((33..126) | Get-Random -Count 64 | ForEach-Object {[char]$_})
```

### Method 2: Online Generator
Visit: https://www.allkeysgenerator.com/Random/Security-Encryption-Key-Generator.aspx
- Choose "256-bit"
- Format: "Hex" or "Base64"

### Method 3: Node.js/JavaScript
```javascript
require('crypto').randomBytes(32).toString('hex')
```

### Method 4: Python
```python
import secrets
secrets.token_urlsafe(32)
```

## Example Strong Keys:

Here are some example 64-character keys you can use:

```
SuperSecureJwtKeyForPortfolioAdminSystem2024WithExtraSecurityPadding!
MyUltraSecurePortfolioJwtKey2024WithSpecialCharactersAndNumbers123@#$
PortfolioAdminSecretKey2024-StrongEnoughForProductionUseWithHS256Alg
```

## For Render Deployment:

Set this as the `JWT_SECRET_KEY` environment variable in your Render dashboard:
```
JWT_SECRET_KEY=SuperSecureJwtKeyForPortfolioAdminSystem2024WithExtraSecurityPadding!
```

**Important**: 
- Minimum 32 characters (256 bits)
- Use mix of letters, numbers, and special characters
- Keep it secret and secure
- Never commit to version control