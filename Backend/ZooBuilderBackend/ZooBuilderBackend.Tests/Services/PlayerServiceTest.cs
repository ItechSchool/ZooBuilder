using Xunit;
using Moq;
using ZooBuilderBackend.Data;
using ZooBuilderBackend.Models;
using ZooBuilderBackend.Services;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ZooBuilderBackend.Tests.Services
{
    public class PlayerServiceTests
    {
        [Fact]
        public void CreatePlayer_ShouldThrowException_WhenDeviceIdIsNull()
        {
            // Arrange
            var mockContext = new Mock<ApplicationDbContext>();
            var service = new PlayerService(mockContext.Object);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => service.Login(null));
            Assert.Equal("Missing Device Id", exception.Message);
        }
    }
}
