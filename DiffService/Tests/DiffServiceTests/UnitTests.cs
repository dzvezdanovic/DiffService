using DiffService.src.DiffService.Api;
using DiffService.src.models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace DiffService.Tests.DiffServiceTests
{
    public class UnitTests
    {
        private readonly DiffController _controller;

        public UnitTests()
        {
            _controller = new DiffController();
        }

        [Fact]
        public void Right_ReturnsCreatedStatusCode()
        {
            // Arrange
            var ID = "testID";
            var diffData = new DiffData { Data = "SGVsbG8gV29ybGQ=" }; // "Hello World" in base64

            // Act
            var result = _controller.Right(ID, diffData) as StatusCodeResult;

            // Assert
            Assert.Equal(201, result.StatusCode);
        }

        [Fact]
        public void Right_ReturnsBadRequest_WhenDataStringIsNull()
        {
            // Arrange
            var ID = "testID";
            DiffData diffData = null;

            // Act
            var result = _controller.Right(ID, diffData) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void Right_ReturnsBadRequest_WhenBase64ConversionFails()
        {
            // Arrange
            var ID = "testID";
            var diffData = new DiffData { Data = "InvalidBase64String" };

            // Act
            var result = _controller.Right(ID, diffData) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }
    }
}
