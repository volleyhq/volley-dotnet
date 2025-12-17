using Xunit;

namespace Volley.Tests
{
    public class VolleyExceptionTests
    {
        [Fact]
        public void Constructor_WithMessage_CreatesException()
        {
            var exception = new VolleyException("Test error");
            Assert.Equal("Test error", exception.Message);
            Assert.Equal(0, exception.StatusCode);
        }

        [Fact]
        public void Constructor_WithMessageAndStatusCode_CreatesException()
        {
            var exception = new VolleyException("Not found", 404);
            Assert.Equal("Not found", exception.Message);
            Assert.Equal(404, exception.StatusCode);
        }

        [Fact]
        public void IsUnauthorized_With401_ReturnsTrue()
        {
            var exception = new VolleyException("Unauthorized", 401);
            Assert.True(exception.IsUnauthorized());
            Assert.False(exception.IsForbidden());
            Assert.False(exception.IsNotFound());
        }

        [Fact]
        public void IsForbidden_With403_ReturnsTrue()
        {
            var exception = new VolleyException("Forbidden", 403);
            Assert.True(exception.IsForbidden());
            Assert.False(exception.IsUnauthorized());
            Assert.False(exception.IsNotFound());
        }

        [Fact]
        public void IsNotFound_With404_ReturnsTrue()
        {
            var exception = new VolleyException("Not found", 404);
            Assert.True(exception.IsNotFound());
            Assert.False(exception.IsUnauthorized());
            Assert.False(exception.IsForbidden());
        }

        [Fact]
        public void IsRateLimited_With429_ReturnsTrue()
        {
            var exception = new VolleyException("Rate limited", 429);
            Assert.True(exception.IsRateLimited());
        }

        [Fact]
        public void IsServerError_With5xx_ReturnsTrue()
        {
            var exception500 = new VolleyException("Server error", 500);
            var exception502 = new VolleyException("Bad gateway", 502);
            var exception503 = new VolleyException("Service unavailable", 503);

            Assert.True(exception500.IsServerError());
            Assert.True(exception502.IsServerError());
            Assert.True(exception503.IsServerError());
        }

        [Fact]
        public void IsServerError_With4xx_ReturnsFalse()
        {
            var exception = new VolleyException("Client error", 400);
            Assert.False(exception.IsServerError());
        }
    }
}

