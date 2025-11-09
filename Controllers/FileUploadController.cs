using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioAdmin.Api.DTOs;
using PortfolioAdmin.Api.Services;

namespace PortfolioAdmin.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FileUploadController : ControllerBase
    {
        private readonly IFileUploadService _fileUploadService;
        private readonly ILogger<FileUploadController> _logger;

        public FileUploadController(IFileUploadService fileUploadService, ILogger<FileUploadController> logger)
        {
            _fileUploadService = fileUploadService;
            _logger = logger;
        }

        [HttpPost("image")]
        public async Task<ActionResult<FileUploadResponseDto>> UploadImage(IFormFile file, [FromQuery] string category = "general")
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest(new { message = "No file provided" });
                }

                if (!_fileUploadService.IsImageFile(file))
                {
                    return BadRequest(new { message = "File must be a valid image format (jpg, jpeg, png, gif, webp)" });
                }

                var url = await _fileUploadService.UploadImageAsync(file, category);
                
                var response = new FileUploadResponseDto
                {
                    FileName = Path.GetFileName(url),
                    Url = url,
                    Category = category,
                    Size = file.Length,
                    UploadedAt = DateTime.UtcNow
                };

                _logger.LogInformation("File uploaded successfully: {FileName} to category: {Category}", file.FileName, category);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file: {FileName}", file.FileName);
                return StatusCode(500, new { message = "Internal server error occurred while uploading the file" });
            }
        }

        [HttpPost("images/bulk")]
        public async Task<ActionResult<IEnumerable<FileUploadResponseDto>>> UploadMultipleImages(
            [FromForm] List<IFormFile> files, 
            [FromQuery] string category = "general")
        {
            try
            {
                if (files == null || !files.Any())
                {
                    return BadRequest(new { message = "No files provided" });
                }

                var responses = new List<FileUploadResponseDto>();
                var errors = new List<string>();

                foreach (var file in files)
                {
                    try
                    {
                        if (!_fileUploadService.IsImageFile(file))
                        {
                            errors.Add($"{file.FileName}: Invalid image format");
                            continue;
                        }

                        var url = await _fileUploadService.UploadImageAsync(file, category);
                        
                        responses.Add(new FileUploadResponseDto
                        {
                            FileName = Path.GetFileName(url),
                            Url = url,
                            Category = category,
                            Size = file.Length,
                            UploadedAt = DateTime.UtcNow
                        });
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"{file.FileName}: {ex.Message}");
                    }
                }

                if (errors.Any())
                {
                    return BadRequest(new { 
                        message = "Some files could not be uploaded", 
                        errors = errors, 
                        uploaded = responses 
                    });
                }

                return Ok(responses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading multiple files");
                return StatusCode(500, new { message = "Internal server error occurred while uploading files" });
            }
        }

        [HttpGet("images")]
        public async Task<ActionResult<IEnumerable<ImageListDto>>> GetUploadedImages([FromQuery] string category = "")
        {
            try
            {
                var images = await _fileUploadService.GetUploadedImagesAsync(category);
                
                var imageDtos = images.Select(url => new ImageListDto
                {
                    Url = url,
                    FileName = Path.GetFileName(url),
                    Category = GetCategoryFromUrl(url)
                });

                return Ok(imageDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving uploaded images");
                return StatusCode(500, new { message = "Internal server error occurred while retrieving images" });
            }
        }

        [HttpDelete("image")]
        public async Task<ActionResult> DeleteImage([FromBody] DeleteFileDto request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.FileName))
                {
                    return BadRequest(new { message = "File name is required" });
                }

                var deleted = await _fileUploadService.DeleteImageAsync(request.FileName);
                
                if (!deleted)
                {
                    return NotFound(new { message = "File not found" });
                }

                return Ok(new { message = "File deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file: {FileName}", request.FileName);
                return StatusCode(500, new { message = "Internal server error occurred while deleting the file" });
            }
        }

        private static string GetCategoryFromUrl(string url)
        {
            var parts = url.Split('/');
            return parts.Length >= 3 ? parts[2] : "general"; // /uploads/category/filename
        }
    }
}