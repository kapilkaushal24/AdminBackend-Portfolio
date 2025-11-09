namespace PortfolioAdmin.Api.DTOs
{
    public record FileUploadResponseDto
    {
        public string FileName { get; init; } = string.Empty;
        public string Url { get; init; } = string.Empty;
        public string Category { get; init; } = string.Empty;
        public long Size { get; init; }
        public DateTime UploadedAt { get; init; } = DateTime.UtcNow;
    }

    public record ImageListDto
    {
        public string Url { get; init; } = string.Empty;
        public string FileName { get; init; } = string.Empty;
        public string Category { get; init; } = string.Empty;
    }

    public record DeleteFileDto
    {
        public string FileName { get; init; } = string.Empty;
    }
}