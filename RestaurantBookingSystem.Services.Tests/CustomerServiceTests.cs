using AutoMapper;
using Moq;
using RestaurantBookingSystem.Data.Models;
using RestaurantBookingSystem.Data.Models.Enums;
using RestaurantBookingSystem.Data.Repository.Contracts;
using RestaurantBookingSystem.Services;
using RestaurantBookingSystem.ViewModels.Customer;
using Xunit;

namespace RestaurantBookingSystem.Tests.Services
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _customerRepo;
        private readonly Mock<IMapper> _mapper;
        private readonly CustomerService _service;

        public CustomerServiceTests()
        {
            _customerRepo = new Mock<ICustomerRepository>();
            _mapper = new Mock<IMapper>();

            _service = new CustomerService(
                _customerRepo.Object,
                _mapper.Object
            );
        }

        #region GetAllCustomersAsync Tests

        [Fact]
        public async Task GetAllCustomersAsync_ReturnsAllCustomersOrderedByName()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer { Id = 1, FullName = "John Doe", PhoneNumber = "111", Reservations = new List<Reservation>() },
                new Customer { Id = 2, FullName = "Alice Smith", PhoneNumber = "222", Reservations = new List<Reservation>() },
                new Customer { Id = 3, FullName = "Bob Johnson", PhoneNumber = "333", Reservations = new List<Reservation>() }
            };

            var viewModels = new List<CustomerIndexViewModel>
            {
                new CustomerIndexViewModel { Id = 1, FullName = "John Doe" },
                new CustomerIndexViewModel { Id = 2, FullName = "Alice Smith" },
                new CustomerIndexViewModel { Id = 3, FullName = "Bob Johnson" }
            };

            _customerRepo.Setup(r => r.GetAllAsync())
                        .ReturnsAsync(customers);

            _mapper.Setup(m => m.Map<List<CustomerIndexViewModel>>(customers))
                  .Returns(viewModels);

            // Act
            var result = await _service.GetAllCustomersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            Assert.Equal("Alice Smith", result.First().FullName);
            Assert.Equal("John Doe", result.Last().FullName);
        }

        [Fact]
        public async Task GetAllCustomersAsync_WithNoCustomers_ReturnsEmptyList()
        {
            // Arrange
            _customerRepo.Setup(r => r.GetAllAsync())
                        .ReturnsAsync(new List<Customer>());

            _mapper.Setup(m => m.Map<List<CustomerIndexViewModel>>(It.IsAny<List<Customer>>()))
                  .Returns(new List<CustomerIndexViewModel>());

            // Act
            var result = await _service.GetAllCustomersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion

        #region GetCustomerDetailsAsync Tests

        [Fact]
        public async Task GetCustomerDetailsAsync_WithValidId_ReturnsDetailsWithReservationHistory()
        {
            // Arrange
            var customer = new Customer
            {
                Id = 1,
                FullName = "John Doe",
                PhoneNumber = "1234567890",
                Email = "john@example.com",
                Status = CustomerStatus.Regular,
                Reservations = new List<Reservation>
                {
                    new Reservation
                    {
                        Id = 1,
                        Date = DateTime.Today.AddDays(1),
                        Time = TimeSpan.FromHours(12),
                        NumberOfGuests = 2,
                        Table = new Table { TableNumber = 5 }
                    },
                    new Reservation
                    {
                        Id = 2,
                        Date = DateTime.Today.AddDays(-1),
                        Time = TimeSpan.FromHours(18),
                        NumberOfGuests = 4,
                        Table = new Table { TableNumber = 10 }
                    }
                }
            };

            var viewModel = new CustomerDetailsViewModel
            {
                Id = 1,
                FullName = "John Doe",
                PhoneNumber = "1234567890",
                Email = "john@example.com",
                Status = CustomerStatus.Regular,
                TotalReservations = 2,
                UpcomingReservations = 1,
                CompletedReservations = 1,
                ReservationHistory = new List<CustomerReservationViewModel>()
            };

            _customerRepo.Setup(r => r.GetByIdWithReservationsAsync(1))
                        .ReturnsAsync(customer);

            _mapper.Setup(m => m.Map<CustomerDetailsViewModel>(customer))
                  .Returns(viewModel);

            // Act
            var result = await _service.GetCustomerDetailsAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("John Doe", result.FullName);
            Assert.Equal(2, result.ReservationHistory.Count);
            Assert.Equal(1, result.ReservationHistory.First().Id);
            Assert.Equal(2, result.ReservationHistory.Last().Id);
        }

        [Fact]
        public async Task GetCustomerDetailsAsync_CustomerNotFound_ReturnsNull()
        {
            // Arrange
            _customerRepo.Setup(r => r.GetByIdWithReservationsAsync(999))
                        .ReturnsAsync((Customer?)null);

            // Act
            var result = await _service.GetCustomerDetailsAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetCustomerDetailsAsync_CustomerWithNoReservations_ReturnsEmptyHistory()
        {
            // Arrange
            var customer = new Customer
            {
                Id = 1,
                FullName = "John Doe",
                PhoneNumber = "1234567890",
                Status = CustomerStatus.Regular,
                Reservations = new List<Reservation>()
            };

            var viewModel = new CustomerDetailsViewModel
            {
                Id = 1,
                FullName = "John Doe",
                TotalReservations = 0,
                ReservationHistory = new List<CustomerReservationViewModel>()
            };

            _customerRepo.Setup(r => r.GetByIdWithReservationsAsync(1))
                        .ReturnsAsync(customer);

            _mapper.Setup(m => m.Map<CustomerDetailsViewModel>(customer))
                  .Returns(viewModel);

            // Act
            var result = await _service.GetCustomerDetailsAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.ReservationHistory);
        }

        #endregion

        #region GetCustomerForEditAsync Tests

        [Fact]
        public async Task GetCustomerForEditAsync_WithValidId_ReturnsEditViewModel()
        {
            // Arrange
            var customer = new Customer
            {
                Id = 1,
                FullName = "John Doe",
                PhoneNumber = "1234567890",
                Email = "john@example.com",
                Status = CustomerStatus.VIP,
                Notes = "Preferred seating by window"
            };

            var viewModel = new CustomerEditViewModel
            {
                Id = 1,
                FullName = "John Doe",
                PhoneNumber = "1234567890",
                Email = "john@example.com",
                Status = CustomerStatus.VIP,
                Notes = "Preferred seating by window"
            };

            _customerRepo.Setup(r => r.GetByIdAsync(1))
                        .ReturnsAsync(customer);

            _mapper.Setup(m => m.Map<CustomerEditViewModel>(customer))
                  .Returns(viewModel);

            // Act
            var result = await _service.GetCustomerForEditAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(CustomerStatus.VIP, result.Status);
            Assert.Equal("Preferred seating by window", result.Notes);
        }

        [Fact]
        public async Task GetCustomerForEditAsync_CustomerNotFound_ReturnsNull()
        {
            // Arrange
            _customerRepo.Setup(r => r.GetByIdAsync(999))
                        .ReturnsAsync((Customer?)null);

            // Act
            var result = await _service.GetCustomerForEditAsync(999);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region UpdateCustomerStatusAsync Tests

        [Fact]
        public async Task UpdateCustomerStatusAsync_WithValidData_UpdatesSuccessfully()
        {
            // Arrange
            var customer = new Customer
            {
                Id = 1,
                FullName = "John Doe",
                PhoneNumber = "1234567890",
                Status = CustomerStatus.Regular,
                Notes = null
            };

            _customerRepo.Setup(r => r.GetByIdAsync(1))
                        .ReturnsAsync(customer);

            var model = new CustomerEditViewModel
            {
                Id = 1,
                FullName = "John Doe",
                PhoneNumber = "1234567890",
                Status = CustomerStatus.VIP,
                Notes = "VIP customer - preferred seating"
            };

            // Act
            var result = await _service.UpdateCustomerStatusAsync(model);

            // Assert
            Assert.True(result);
            _customerRepo.Verify(r => r.Update(It.Is<Customer>(c =>
                c.Status == CustomerStatus.VIP &&
                c.Notes == "VIP customer - preferred seating")), Times.Once);
            _customerRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateCustomerStatusAsync_CustomerNotFound_ReturnsFalse()
        {
            // Arrange
            _customerRepo.Setup(r => r.GetByIdAsync(999))
                        .ReturnsAsync((Customer?)null);

            var model = new CustomerEditViewModel
            {
                Id = 999,
                Status = CustomerStatus.VIP
            };

            // Act
            var result = await _service.UpdateCustomerStatusAsync(model);

            // Assert
            Assert.False(result);
            _customerRepo.Verify(r => r.Update(It.IsAny<Customer>()), Times.Never);
            _customerRepo.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateCustomerStatusAsync_ChangeToBlacklisted_UpdatesSuccessfully()
        {
            // Arrange
            var customer = new Customer
            {
                Id = 1,
                FullName = "Bad Customer",
                PhoneNumber = "1234567890",
                Status = CustomerStatus.Regular
            };

            _customerRepo.Setup(r => r.GetByIdAsync(1))
                        .ReturnsAsync(customer);

            var model = new CustomerEditViewModel
            {
                Id = 1,
                Status = CustomerStatus.Blacklisted,
                Notes = "Multiple no-shows"
            };

            // Act
            var result = await _service.UpdateCustomerStatusAsync(model);

            // Assert
            Assert.True(result);
            _customerRepo.Verify(r => r.Update(It.Is<Customer>(c =>
                c.Status == CustomerStatus.Blacklisted)), Times.Once);
        }

        #endregion

        #region SearchCustomersAsync Tests

        [Fact]
        public async Task SearchCustomersAsync_WithMatchingName_ReturnsMatchingCustomers()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer { Id = 1, FullName = "John Doe", PhoneNumber = "111", Reservations = new List<Reservation>() },
                new Customer { Id = 2, FullName = "John Smith", PhoneNumber = "222", Reservations = new List<Reservation>() }
            };

            var viewModels = new List<CustomerIndexViewModel>
            {
                new CustomerIndexViewModel { Id = 1, FullName = "John Doe" },
                new CustomerIndexViewModel { Id = 2, FullName = "John Smith" }
            };

            _customerRepo.Setup(r => r.SearchAsync("john"))
                        .ReturnsAsync(customers);

            _mapper.Setup(m => m.Map<List<CustomerIndexViewModel>>(customers))
                  .Returns(viewModels);

            // Act
            var result = await _service.SearchCustomersAsync("john");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task SearchCustomersAsync_NoMatches_ReturnsEmptyList()
        {
            // Arrange
            _customerRepo.Setup(r => r.SearchAsync("xyz"))
                        .ReturnsAsync(new List<Customer>());

            _mapper.Setup(m => m.Map<List<CustomerIndexViewModel>>(It.IsAny<List<Customer>>()))
                  .Returns(new List<CustomerIndexViewModel>());

            // Act
            var result = await _service.SearchCustomersAsync("xyz");

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion
    }
}