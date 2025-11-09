using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace PortfolioAdmin.Api.Tests
{
    public class HealthControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public HealthControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Get_HealthCheck_ReturnsSuccess()
        {
            // Act
            var response = await _client.GetAsync("/api/health");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("healthy", content);
        }

        [Fact]
        public async Task Get_DetailedHealthCheck_ReturnsSuccess()
        {
            // Act
            var response = await _client.GetAsync("/api/health/detailed");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("healthy", content);
            Assert.Contains("memory", content);
            Assert.Contains("uptime", content);
        }
    }
}