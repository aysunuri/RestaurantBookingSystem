using AutoMapper;
using Moq;
using RestaurantBookingSystem.Data.Models;
using RestaurantBookingSystem.Data.Repository.Contracts;
using RestaurantBookingSystem.Services;
using RestaurantBookingSystem.ViewModels.Events;
using Xunit;

namespace RestaurantBookingSystem.Tests.Services
{
    public class EventServiceTests
    {
        private readonly Mock<IEventRepository> _eventRepo;
        private readonly Mock<IMapper> _mapper;
        private readonly EventService _service;

        public EventServiceTests()
        {
            _eventRepo = new Mock<IEventRepository>();
            _mapper = new Mock<IMapper>();

            _service = new EventService(
                _eventRepo.Object,
                _mapper.Object
            );
        }

        #region GetAllEventsAsync Tests

        [Fact]
        public async Task GetAllEventsAsync_ReturnsAllEvents()
        {
            // Arrange
            var events = new List<Event>
            {
                new Event { Id = 1, Name = "Pizza Day", Date = DateTime.Today.AddDays(5), IsActive = true },
                new Event { Id = 2, Name = "Taco Tuesday", Date = DateTime.Today.AddDays(10), IsActive = false }
            };

            var viewModels = new List<EventIndexViewModel>
            {
                new EventIndexViewModel { Id = 1, Name = "Pizza Day" },
                new EventIndexViewModel { Id = 2, Name = "Taco Tuesday" }
            };

            _eventRepo.Setup(r => r.GetAllAsync())
                     .ReturnsAsync(events);

            _mapper.Setup(m => m.Map<List<EventIndexViewModel>>(events))
                  .Returns(viewModels);

            // Act
            var result = await _service.GetAllEventsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        #endregion

        #region GetActiveEventsAsync Tests

        [Fact]
        public async Task GetActiveEventsAsync_ReturnsOnlyActiveEvents()
        {
            // Arrange
            var events = new List<Event>
            {
                new Event { Id = 1, Name = "Pizza Day", Date = DateTime.Today.AddDays(5), IsActive = true }
            };

            var viewModels = new List<EventIndexViewModel>
            {
                new EventIndexViewModel { Id = 1, Name = "Pizza Day" }
            };

            _eventRepo.Setup(r => r.GetActiveEventsAsync())
                     .ReturnsAsync(events);

            _mapper.Setup(m => m.Map<List<EventIndexViewModel>>(events))
                  .Returns(viewModels);

            // Act
            var result = await _service.GetActiveEventsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        #endregion

        #region GetEventDetailsAsync Tests

        [Fact]
        public async Task GetEventDetailsAsync_WithValidId_ReturnsDetails()
        {
            // Arrange
            var eventEntity = new Event
            {
                Id = 1,
                Name = "Pizza Day",
                Description = "All pizzas 20% off",
                Date = DateTime.Today.AddDays(5),
                IsActive = true
            };

            var viewModel = new EventDetailsViewModel
            {
                Id = 1,
                Name = "Pizza Day",
                Description = "All pizzas 20% off"
            };

            _eventRepo.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync(eventEntity);

            _mapper.Setup(m => m.Map<EventDetailsViewModel>(eventEntity))
                  .Returns(viewModel);

            // Act
            var result = await _service.GetEventDetailsAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Pizza Day", result.Name);
        }

        [Fact]
        public async Task GetEventDetailsAsync_EventNotFound_ReturnsNull()
        {
            // Arrange
            _eventRepo.Setup(r => r.GetByIdAsync(999))
                     .ReturnsAsync((Event?)null);

            // Act
            var result = await _service.GetEventDetailsAsync(999);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region CreateEventAsync Tests

        [Fact]
        public async Task CreateEventAsync_AddsEventSuccessfully()
        {
            // Arrange
            var model = new EventFormViewModel
            {
                Name = "Ladies Night",
                Description = "Special cocktails",
                Date = DateTime.Today.AddDays(15),
                IsActive = true
            };

            var eventEntity = new Event
            {
                Name = "Ladies Night",
                Description = "Special cocktails",
                Date = DateTime.Today.AddDays(15),
                IsActive = true
            };

            _mapper.Setup(m => m.Map<Event>(model))
                  .Returns(eventEntity);

            // Act
            await _service.CreateEventAsync(model);

            // Assert
            _eventRepo.Verify(r => r.AddAsync(It.IsAny<Event>()), Times.Once);
            _eventRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        #endregion

        #region UpdateEventAsync Tests

        [Fact]
        public async Task UpdateEventAsync_WithValidData_UpdatesSuccessfully()
        {
            // Arrange
            var eventEntity = new Event
            {
                Id = 1,
                Name = "Pizza Day",
                Date = DateTime.Today.AddDays(5),
                IsActive = true
            };

            var model = new EventFormViewModel
            {
                Id = 1,
                Name = "Super Pizza Day",
                Date = DateTime.Today.AddDays(10),
                IsActive = true
            };

            _eventRepo.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync(eventEntity);

            _mapper.Setup(m => m.Map(model, eventEntity))
                  .Returns(eventEntity);

            // Act
            var result = await _service.UpdateEventAsync(model);

            // Assert
            Assert.True(result);
            _eventRepo.Verify(r => r.Update(It.IsAny<Event>()), Times.Once);
            _eventRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateEventAsync_EventNotFound_ReturnsFalse()
        {
            // Arrange
            _eventRepo.Setup(r => r.GetByIdAsync(999))
                     .ReturnsAsync((Event?)null);

            var model = new EventFormViewModel { Id = 999, Name = "Test" };

            // Act
            var result = await _service.UpdateEventAsync(model);

            // Assert
            Assert.False(result);
            _eventRepo.Verify(r => r.Update(It.IsAny<Event>()), Times.Never);
        }

        #endregion

        #region DeleteEventAsync Tests

        [Fact]
        public async Task DeleteEventAsync_WithValidId_DeletesSuccessfully()
        {
            // Arrange
            var eventEntity = new Event { Id = 1, Name = "Pizza Day" };

            _eventRepo.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync(eventEntity);

            // Act
            var result = await _service.DeleteEventAsync(1);

            // Assert
            Assert.True(result);
            _eventRepo.Verify(r => r.Delete(It.IsAny<Event>()), Times.Once);
            _eventRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteEventAsync_EventNotFound_ReturnsFalse()
        {
            // Arrange
            _eventRepo.Setup(r => r.GetByIdAsync(999))
                     .ReturnsAsync((Event?)null);

            // Act
            var result = await _service.DeleteEventAsync(999);

            // Assert
            Assert.False(result);
            _eventRepo.Verify(r => r.Delete(It.IsAny<Event>()), Times.Never);
        }

        #endregion
    }
}