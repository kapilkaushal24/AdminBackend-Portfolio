namespace PortfolioAdmin.Api.Services
{
    public interface IFileUploadService
    {
        Task<string> UploadImageAsync(IFormFile file, string category = "general");
        Task<bool> DeleteImageAsync(string fileName);
        bool IsImageFile(IFormFile file);
        Task<IEnumerable<string>> GetUploadedImagesAsync(string category = "");
    }

    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<FileUploadService> _logger;
        private readonly string _uploadsPath;
        private readonly long _maxFileSize = 5 * 1024 * 1024; // 5MB
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        public FileUploadService(IWebHostEnvironment environment, ILogger<FileUploadService> logger)
        {
            _environment = environment;
            _logger = logger;
            _uploadsPath = Path.Combine(_environment.WebRootPath ?? _environment.ContentRootPath, "uploads");
            
            // Ensure uploads directory exists
            if (!Directory.Exists(_uploadsPath))
            {
                Directory.CreateDirectory(_uploadsPath);
            }
        }

        public async Task<string> UploadImageAsync(IFormFile file, string category = "general")
        {
            if (!IsImageFile(file))
            {
                throw new ArgumentException("File is not a valid image format");
            }

            if (file.Length > _maxFileSize)
            {
                throw new ArgumentException($"File size exceeds maximum allowed size of {_maxFileSize / (1024 * 1024)}MB");
            }

            // Create category directory if it doesn't exist
            var categoryPath = Path.Combine(_uploadsPath, category);
            if (!Directory.Exists(categoryPath))
            {
                Directory.CreateDirectory(categoryPath);
            }

            // Generate unique filename
            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(categoryPath, fileName);

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Return relative URL path
                return $"/uploads/{category}/{fileName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file {FileName}", file.FileName);
                throw new InvalidOperationException("Error occurred while uploading the file");
            }
        }

        public Task<bool> DeleteImageAsync(string fileName)
        {
            try
            {
                // Extract relative path and convert to physical path
                var relativePath = fileName.TrimStart('/');
                var filePath = Path.Combine(_environment.WebRootPath ?? _environment.ContentRootPath, relativePath);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    _logger.LogInformation("Deleted file: {FilePath}", filePath);
                    return Task.FromResult(true);
                }

                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file {FileName}", fileName);
                return Task.FromResult(false);
            }
        }

        public bool IsImageFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            if (string.IsNullOrEmpty(extension))
                return false;

            return _allowedExtensions.Contains(extension);
        }

        public Task<IEnumerable<string>> GetUploadedImagesAsync(string category = "")
        {
            try
            {
                var searchPath = string.IsNullOrEmpty(category) 
                    ? _uploadsPath 
                    : Path.Combine(_uploadsPath, category);

                if (!Directory.Exists(searchPath))
                    return Task.FromResult(Enumerable.Empty<string>());

                var files = Directory.GetFiles(searchPath, "*", SearchOption.AllDirectories)
                    .Where(f => _allowedExtensions.Contains(Path.GetExtension(f).ToLowerInvariant()))
                    .Select(f => f.Replace(_environment.WebRootPath ?? _environment.ContentRootPath, "").Replace('\\', '/'))
                    .ToList();

                return Task.FromResult(files.AsEnumerable());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving uploaded images");
                return Task.FromResult(Enumerable.Empty<string>());
            }
        }
    }
}