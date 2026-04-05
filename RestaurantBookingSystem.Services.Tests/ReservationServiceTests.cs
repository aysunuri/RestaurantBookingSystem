using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using RestaurantBookingSystem.Data.Models;
using RestaurantBookingSystem.Data.Models.Enums;
using RestaurantBookingSystem.Data.Repository.Contracts;
using RestaurantBookingSystem.Services;
using RestaurantBookingSystem.ViewModels.Reservation;
using RestaurantBookingSystem.ViewModels.Shared;
using Xunit;

namespace RestaurantBookingSystem.Tests.Services
{
    public class ReservationServiceTests
    {
        private readonly Mock<IReservationRepository> _reservationRepo;
        private readonly Mock<ICustomerRepository> _customerRepo;
        private readonly Mock<ITableRepository> _tableRepo;
        private readonly Mock<ISettingsRepository> _settingsRepo;
        private readonly Mock<IMapper> _mapper;
        private readonly ReservationService _service;

        public ReservationServiceTests()
        {
            _reservationRepo = new Mock<IReservationRepository>();
            _customerRepo = new Mock<ICustomerRepository>();
            _tableRepo = new Mock<ITableRepository>();
            _settingsRepo = new Mock<ISettingsRepository>();
            _mapper = new Mock<IMapper>();

            SetupDefaultMocks();

            _service = new ReservationService(
                _reservationRepo.Object,
                _customerRepo.Object,
                _tableRepo.Object,
                _settingsRepo.Object,
                _mapper.Object
            );
        }

        private void SetupDefaultMocks()
        {
            _tableRepo.Setup(t => t.TableHasEnoughSeatsAsync(It.IsAny<int>(), It.IsAny<int>()))
                     .ReturnsAsync(true);

            _reservationRepo.Setup(r => r.IsTableAvailableAsync(
                    It.IsAny<int>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<TimeSpan>(),
                    It.IsAny<int?>()))
                .ReturnsAsync(true);

            _settingsRepo.Setup(s => s.GetSettingsAsync())
                .ReturnsAsync(new RestaurantSettings
                {
                    Id = 1,
                    RestaurantName = "Test Restaurant",
                    OpeningHour = TimeSpan.FromHours(10),
                    ClosingHour = TimeSpan.FromHours(23)
                });

            _customerRepo.Setup(c => c.GetByPhoneNumberAsync(It.IsAny<string>()))
                        .ReturnsAsync((Customer?)null);
        }

        #region AddReservationAsync Tests

        [Fact]
        public async Task AddReservationAsync_WithValidData_AddsReservationSuccessfully()
        {
            // Arrange
            var model = new ReservationFormViewModel
            {
                Date = DateTime.Today.AddDays(1),
                Time = TimeSpan.FromHours(12),
                NumberOfGuests = 2,
                TableId = 1,
                CustomerName = "John Doe",
                CustomerPhone = "1234567890",
                CustomerEmail = "john@example.com"
            };

            // Act
            await _service.AddReservationAsync(model);

            // Assert
            _customerRepo.Verify(c => c.AddAsync(It.IsAny<Customer>()), Times.Once);
            _reservationRepo.Verify(r => r.AddAsync(It.IsAny<Reservation>()), Times.Once);
            _reservationRepo.Verify(r => r.SaveChangesAsync(), Times.Exactly(2));
        }

        [Fact]
        public async Task AddReservationAsync_WithExistingCustomer_DoesNotCreateNewCustomer()
        {
            // Arrange
            _customerRepo.Setup(c => c.GetByPhoneNumberAsync("1234567890"))
                .ReturnsAsync(new Customer
                {
                    Id = 1,
                    FullName = "John Doe",
                    PhoneNumber = "1234567890",
                    Status = CustomerStatus.Regular
                });

            var model = new ReservationFormViewModel
            {
                Date = DateTime.Today.AddDays(1),
                Time = TimeSpan.FromHours(12),
                NumberOfGuests = 2,
                TableId = 1,
                CustomerName = "John Doe",
                CustomerPhone = "1234567890",
                CustomerEmail = "john@example.com"
            };

            // Act
            await _service.AddReservationAsync(model);

            // Assert
            _customerRepo.Verify(c => c.AddAsync(It.IsAny<Customer>()), Times.Never);
            _reservationRepo.Verify(r => r.AddAsync(It.IsAny<Reservation>()), Times.Once);
        }

        [Fact]
        public async Task AddReservationAsync_VIPCustomer_CreatesReservationSuccessfully()
        {
            // Arrange
            _customerRepo.Setup(c => c.GetByPhoneNumberAsync("1234567890"))
                .ReturnsAsync(new Customer
                {
                    Id = 1,
                    FullName = "VIP Customer",
                    PhoneNumber = "1234567890",
                    Status = CustomerStatus.VIP
                });

            var model = new ReservationFormViewModel
            {
                Date = DateTime.Today.AddDays(1),
                Time = TimeSpan.FromHours(12),
                NumberOfGuests = 2,
                TableId = 1,
                CustomerName = "VIP Customer",
                CustomerPhone = "1234567890"
            };

            // Act
            await _service.AddReservationAsync(model);

            // Assert
            _reservationRepo.Verify(r => r.AddAsync(It.IsAny<Reservation>()), Times.Once);
        }

        [Fact]
        public async Task AddReservationAsync_WithPastDate_ThrowsInvalidOperationException()
        {
            // Arrange
            var model = new ReservationFormViewModel
            {
                Date = DateTime.Today.AddDays(-1),
                Time = TimeSpan.FromHours(12),
                NumberOfGuests = 2,
                TableId = 1,
                CustomerName = "John Doe",
                CustomerPhone = "1234567890"
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.AddReservationAsync(model));

            Assert.Equal("Cannot create reservations for past dates or times.", exception.Message);
        }

        [Fact]
        public async Task AddReservationAsync_OutsideOperatingHours_ThrowsInvalidOperationException()
        {
            // Arrange
            var model = new ReservationFormViewModel
            {
                Date = DateTime.Today.AddDays(1),
                Time = TimeSpan.FromHours(6),
                NumberOfGuests = 2,
                TableId = 1,
                CustomerName = "John Doe",
                CustomerPhone = "1234567890"
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.AddReservationAsync(model));
        }

        [Fact]
        public async Task AddReservationAsync_AfterLastSeating_ThrowsInvalidOperationException()
        {
            // Arrange
            var model = new ReservationFormViewModel
            {
                Date = DateTime.Today.AddDays(1),
                Time = TimeSpan.FromHours(22).Add(TimeSpan.FromMinutes(30)),
                NumberOfGuests = 2,
                TableId = 1,
                CustomerName = "John Doe",
                CustomerPhone = "1234567890"
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.AddReservationAsync(model));
        }

        [Fact]
        public async Task AddReservationAsync_TableNotEnoughSeats_ThrowsInvalidOperationException()
        {
            // Arrange
            _tableRepo.Setup(t => t.TableHasEnoughSeatsAsync(1, 10))
                     .ReturnsAsync(false);

            var model = new ReservationFormViewModel
            {
                Date = DateTime.Today.AddDays(1),
                Time = TimeSpan.FromHours(12),
                NumberOfGuests = 10,
                TableId = 1,
                CustomerName = "John Doe",
                CustomerPhone = "1234567890"
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.AddReservationAsync(model));

            Assert.Equal("The selected table doesn't have enough seats for the number of guests.", exception.Message);
        }

        [Fact]
        public async Task AddReservationAsync_TableNotAvailable_ThrowsInvalidOperationException()
        {
            // Arrange
            _reservationRepo.Setup(r => r.IsTableAvailableAsync(1, It.IsAny<DateTime>(), It.IsAny<TimeSpan>(), null))
                           .ReturnsAsync(false);

            var model = new ReservationFormViewModel
            {
                Date = DateTime.Today.AddDays(1),
                Time = TimeSpan.FromHours(12),
                NumberOfGuests = 2,
                TableId = 1,
                CustomerName = "John Doe",
                CustomerPhone = "1234567890"
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.AddReservationAsync(model));

            Assert.Equal("The selected table is already booked for this time slot.", exception.Message);
        }

        [Fact]
        public async Task AddReservationAsync_BlacklistedCustomer_ThrowsInvalidOperationException()
        {
            // Arrange
            _customerRepo.Setup(c => c.GetByPhoneNumberAsync("1234567890"))
                .ReturnsAsync(new Customer
                {
                    Id = 1,
                    FullName = "Bad Customer",
                    PhoneNumber = "1234567890",
                    Status = CustomerStatus.Blacklisted
                });

            var model = new ReservationFormViewModel
            {
                Date = DateTime.Today.AddDays(1),
                Time = TimeSpan.FromHours(12),
                NumberOfGuests = 2,
                TableId = 1,
                CustomerName = "Bad Customer",
                CustomerPhone = "1234567890"
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.AddReservationAsync(model));

            Assert.Contains("blacklisted", exception.Message.ToLower());
        }

        #endregion

        #region EditReservationAsync Tests

        [Fact]
        public async Task EditReservationAsync_WithValidData_UpdatesReservationSuccessfully()
        {
            // Arrange
            var reservation = new Reservation
            {
                Id = 1,
                Date = DateTime.Today.AddDays(1),
                Time = TimeSpan.FromHours(12),
                NumberOfGuests = 2,
                TableId = 1,
                CustomerId = 1,
                Customer = new Customer
                {
                    Id = 1,
                    FullName = "John Doe",
                    PhoneNumber = "1234567890",
                    Email = "john@example.com",
                    Status = CustomerStatus.Regular
                }
            };

            _reservationRepo.Setup(r => r.GetByIdWithCustomerAsync(1))
                           .ReturnsAsync(reservation);

            var model = new ReservationFormViewModel
            {
                Id = 1,
                Date = DateTime.Today.AddDays(2),
                Time = TimeSpan.FromHours(14),
                NumberOfGuests = 4,
                TableId = 1,
                CustomerName = "John Doe",
                CustomerPhone = "1234567890",
                CustomerEmail = "john@example.com"
            };

            // Act
            var result = await _service.EditReservationAsync(model);

            // Assert
            Assert.True(result);
            _reservationRepo.Verify(r => r.Update(It.IsAny<Reservation>()), Times.Once);
            _reservationRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task EditReservationAsync_ReservationNotFound_ReturnsFalse()
        {
            // Arrange
            _reservationRepo.Setup(r => r.GetByIdWithCustomerAsync(999))
                           .ReturnsAsync((Reservation?)null);

            var model = new ReservationFormViewModel
            {
                Id = 999,
                Date = DateTime.Today.AddDays(1),
                Time = TimeSpan.FromHours(12),
                NumberOfGuests = 2,
                TableId = 1,
                CustomerName = "John Doe",
                CustomerPhone = "1234567890"
            };

            // Act
            var result = await _service.EditReservationAsync(model);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task EditReservationAsync_ChangeCustomerInfo_UpdatesCorrectly()
        {
            // Arrange
            var reservation = new Reservation
            {
                Id = 1,
                Date = DateTime.Today.AddDays(1),
                Time = TimeSpan.FromHours(12),
                NumberOfGuests = 2,
                TableId = 1,
                CustomerId = 1,
                Customer = new Customer
                {
                    Id = 1,
                    FullName = "John Doe",
                    PhoneNumber = "1111111111",
                    Email = "john@old.com",
                    Status = CustomerStatus.Regular
                }
            };

            _reservationRepo.Setup(r => r.GetByIdWithCustomerAsync(1))
                           .ReturnsAsync(reservation);

            _customerRepo.Setup(c => c.GetByExactMatchAsync(
                    "Jane Smith", "2222222222", "jane@new.com"))
                .ReturnsAsync((Customer?)null);

            _customerRepo.Setup(c => c.GetByPhoneNumberAsync("2222222222"))
                .ReturnsAsync((Customer?)null);

            var model = new ReservationFormViewModel
            {
                Id = 1,
                Date = DateTime.Today.AddDays(2),
                Time = TimeSpan.FromHours(14),
                NumberOfGuests = 4,
                TableId = 1,
                CustomerName = "Jane Smith",
                CustomerPhone = "2222222222",
                CustomerEmail = "jane@new.com"
            };

            // Act
            var result = await _service.EditReservationAsync(model);

            // Assert
            Assert.True(result);
            _customerRepo.Verify(c => c.AddAsync(It.IsAny<Customer>()), Times.Once);
        }

        [Fact]
        public async Task EditReservationAsync_PastDate_ThrowsException()
        {
            // Arrange
            var reservation = new Reservation
            {
                Id = 1,
                Date = DateTime.Today.AddDays(1),
                Time = TimeSpan.FromHours(12),
                NumberOfGuests = 2,
                TableId = 1,
                CustomerId = 1,
                Customer = new Customer
                {
                    Id = 1,
                    FullName = "John Doe",
                    PhoneNumber = "1234567890",
                    Status = CustomerStatus.Regular
                }
            };

            _reservationRepo.Setup(r => r.GetByIdWithCustomerAsync(1))
                           .ReturnsAsync(reservation);

            var model = new ReservationFormViewModel
            {
                Id = 1,
                Date = DateTime.Today.AddDays(-1),
                Time = TimeSpan.FromHours(12),
                NumberOfGuests = 2,
                TableId = 1,
                CustomerName = "John Doe",
                CustomerPhone = "1234567890"
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.EditReservationAsync(model));
        }

        [Fact]
        public async Task EditReservationAsync_TableNotAvailable_ThrowsException()
        {
            // Arrange
            var reservation = new Reservation
            {
                Id = 1,
                Date = DateTime.Today.AddDays(1),
                Time = TimeSpan.FromHours(12),
                NumberOfGuests = 2,
                TableId = 1,
                CustomerId = 1,
                Customer = new Customer
                {
                    Id = 1,
                    FullName = "John Doe",
                    PhoneNumber = "1234567890",
                    Status = CustomerStatus.Regular
                }
            };

            _reservationRepo.Setup(r => r.GetByIdWithCustomerAsync(1))
                           .ReturnsAsync(reservation);

            _reservationRepo.Setup(r => r.IsTableAvailableAsync(
                    2, It.IsAny<DateTime>(), It.IsAny<TimeSpan>(), 1))
                .ReturnsAsync(false);

            var model = new ReservationFormViewModel
            {
                Id = 1,
                Date = DateTime.Today.AddDays(1),
                Time = TimeSpan.FromHours(12),
                NumberOfGuests = 2,
                TableId = 2,
                CustomerName = "John Doe",
                CustomerPhone = "1234567890"
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.EditReservationAsync(model));
        }

        [Fact]
        public async Task EditReservationAsync_WithBlacklistedCustomer_ThrowsInvalidOperationException()
        {
            // Arrange
            var reservation = new Reservation
            {
                Id = 1,
                Date = DateTime.Today.AddDays(1),
                Time = TimeSpan.FromHours(12),
                NumberOfGuests = 2,
                TableId = 1,
                CustomerId = 1,
                Customer = new Customer
                {
                    Id = 1,
                    FullName = "Bad Customer",
                    PhoneNumber = "1234567890",
                    Status = CustomerStatus.Blacklisted
                }
            };

            _reservationRepo.Setup(r => r.GetByIdWithCustomerAsync(1))
                           .ReturnsAsync(reservation);

            var model = new ReservationFormViewModel
            {
                Id = 1,
                Date = DateTime.Today.AddDays(2),
                Time = TimeSpan.FromHours(14),
                NumberOfGuests = 4,
                TableId = 1,
                CustomerName = "Bad Customer",
                CustomerPhone = "1234567890"
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.EditReservationAsync(model));
        }

        #endregion

        #region DeleteReservationAsync Tests

        [Fact]
        public async Task DeleteReservationAsync_WithValidId_DeletesSuccessfully()
        {
            // Arrange
            var reservation = new Reservation { Id = 1 };
            _reservationRepo.Setup(r => r.GetByIdAsync(1))
                           .ReturnsAsync(reservation);

            // Act
            var result = await _service.DeleteReservationAsync(1);

            // Assert
            Assert.True(result);
            _reservationRepo.Verify(r => r.Delete(It.IsAny<Reservation>()), Times.Once);
            _reservationRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteReservationAsync_ReservationNotFound_ReturnsFalse()
        {
            // Arrange
            _reservationRepo.Setup(r => r.GetByIdAsync(999))
                           .ReturnsAsync((Reservation?)null);

            // Act
            var result = await _service.DeleteReservationAsync(999);

            // Assert
            Assert.False(result);
            _reservationRepo.Verify(r => r.Delete(It.IsAny<Reservation>()), Times.Never);
        }

        #endregion

        #region GetReservationDetailsAsync Tests

        [Fact]
        public async Task GetReservationDetailsAsync_WithValidId_ReturnsViewModel()
        {
            // Arrange
            var reservation = new Reservation
            {
                Id = 1,
                Date = DateTime.Today,
                Time = TimeSpan.FromHours(12),
                NumberOfGuests = 2
            };

            var viewModel = new ReservationDetailsViewModel
            {
                Id = 1,
                Date = DateTime.Today.ToString(),
                Time = TimeSpan.FromHours(12).ToString(),
                NumberOfGuests = 2
            };

            _reservationRepo.Setup(r => r.GetByIdWithDetailsAsync(1))
                           .ReturnsAsync(reservation);

            _mapper.Setup(m => m.Map<ReservationDetailsViewModel>(reservation))
                  .Returns(viewModel);

            // Act
            var result = await _service.GetReservationDetailsAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task GetReservationDetailsAsync_NotFound_ReturnsNull()
        {
            // Arrange
            _reservationRepo.Setup(r => r.GetByIdWithDetailsAsync(999))
                           .ReturnsAsync((Reservation?)null);

            // Act
            var result = await _service.GetReservationDetailsAsync(999);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region IsValidReservationDateTime Tests

        [Fact]
        public void IsValidReservationDateTime_FutureDateTime_ReturnsTrue()
        {
            // Arrange
            var futureDate = DateTime.Today.AddDays(1);
            var futureTime = TimeSpan.FromHours(12);

            // Act
            var result = _service.IsValidReservationDateTime(futureDate, futureTime);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidReservationDateTime_PastDateTime_ReturnsFalse()
        {
            // Arrange
            var pastDate = DateTime.Today.AddDays(-1);
            var pastTime = TimeSpan.FromHours(12);

            // Act
            var result = _service.IsValidReservationDateTime(pastDate, pastTime);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region Helper Method Tests

        [Fact]
        public async Task IsWithinOperatingHoursAsync_ValidTime_ReturnsTrue()
        {
            var result = await _service.IsWithinOperatingHoursAsync(TimeSpan.FromHours(12));
            Assert.True(result);
        }

        [Fact]
        public async Task TableIsAvailableAsync_ReturnsTrue()
        {
            var result = await _service.TableIsAvailableAsync(1, DateTime.Today, TimeSpan.FromHours(12));
            Assert.True(result);
        }

        #endregion
    }
}