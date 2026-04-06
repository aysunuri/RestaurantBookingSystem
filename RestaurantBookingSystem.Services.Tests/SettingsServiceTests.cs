using AutoMapper;
using Moq;
using RestaurantBookingSystem.Data.Models;
using RestaurantBookingSystem.Data.Repository.Contracts;
using RestaurantBookingSystem.Services;
using RestaurantBookingSystem.ViewModels.Settings;
using Xunit;

namespace RestaurantBookingSystem.Tests.Services
{
    public class SettingsServiceTests
    {
        private readonly Mock<ISettingsRepository> _settingsRepo;
        private readonly Mock<IMapper> _mapper;
        private readonly SettingsService _service;

        public SettingsServiceTests()
        {
            _settingsRepo = new Mock<ISettingsRepository>();
            _mapper = new Mock<IMapper>();

            _service = new SettingsService(
                _settingsRepo.Object,
                _mapper.Object
            );
        }

        #region GetSettingsAsync Tests

        [Fact]
        public async Task GetSettingsAsync_ReturnsSettings()
        {
            // Arrange
            var settings = new RestaurantSettings
            {
                Id = 1,
                RestaurantName = "Test Restaurant",
                OpeningHour = TimeSpan.FromHours(10),
                ClosingHour = TimeSpan.FromHours(22)
            };

            var viewModel = new SettingsViewModel
            {
                Id = 1,
                RestaurantName = "Test Restaurant",
                OpeningHour = TimeSpan.FromHours(10),
                ClosingHour = TimeSpan.FromHours(22)
            };

            _settingsRepo.Setup(r => r.GetSettingsAsync())
                        .ReturnsAsync(settings);

            _mapper.Setup(m => m.Map<SettingsViewModel>(settings))
                  .Returns(viewModel);

            // Act
            var result = await _service.GetSettingsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Restaurant", result.RestaurantName);
        }

        [Fact]
        public async Task GetSettingsAsync_NoSettings_ReturnsNull()
        {
            // Arrange
            _settingsRepo.Setup(r => r.GetSettingsAsync())
                        .ReturnsAsync((RestaurantSettings?)null);

            // Act
            var result = await _service.GetSettingsAsync();

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region UpdateSettingsAsync Tests

        [Fact]
        public async Task UpdateSettingsAsync_WithValidData_UpdatesSuccessfully()
        {
            // Arrange
            var settings = new RestaurantSettings
            {
                Id = 1,
                RestaurantName = "Old Name",
                OpeningHour = TimeSpan.FromHours(10),
                ClosingHour = TimeSpan.FromHours(22)
            };

            var model = new SettingsViewModel
            {
                Id = 1,
                RestaurantName = "New Name",
                OpeningHour = TimeSpan.FromHours(9),
                ClosingHour = TimeSpan.FromHours(23)
            };

            _settingsRepo.Setup(r => r.GetSettingsAsync())
                        .ReturnsAsync(settings);

            _mapper.Setup(m => m.Map(model, settings))
                  .Returns(settings);

            // Act
            var result = await _service.UpdateSettingsAsync(model);

            // Assert
            Assert.True(result);
            _settingsRepo.Verify(r => r.Update(It.IsAny<RestaurantSettings>()), Times.Once);
            _settingsRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateSettingsAsync_NoSettings_ReturnsFalse()
        {
            // Arrange
            _settingsRepo.Setup(r => r.GetSettingsAsync())
                        .ReturnsAsync((RestaurantSettings?)null);

            var model = new SettingsViewModel
            {
                Id = 1,
                RestaurantName = "Test"
            };

            // Act
            var result = await _service.UpdateSettingsAsync(model);

            // Assert
            Assert.False(result);
            _settingsRepo.Verify(r => r.Update(It.IsAny<RestaurantSettings>()), Times.Never);
        }

        #endregion
    }
}