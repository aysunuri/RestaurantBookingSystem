using AutoMapper;
using Moq;
using RestaurantBookingSystem.Data.Models;
using RestaurantBookingSystem.Data.Repository.Contracts;
using RestaurantBookingSystem.Services;
using RestaurantBookingSystem.ViewModels.Tables;
using Xunit;

namespace RestaurantBookingSystem.Tests.Services
{
    public class TableServiceTests
    {
        private readonly Mock<ITableRepository> _tableRepo;
        private readonly Mock<IMapper> _mapper;
        private readonly TableService _service;

        public TableServiceTests()
        {
            _tableRepo = new Mock<ITableRepository>();
            _mapper = new Mock<IMapper>();

            _service = new TableService(
                _tableRepo.Object,
                _mapper.Object
            );
        }

        #region GetAllTablesAsync Tests

        [Fact]
        public async Task GetAllTablesAsync_ReturnsAllTables()
        {
            // Arrange
            var tables = new List<Table>
            {
                new Table { Id = 1, TableNumber = 5, Seats = 4 },
                new Table { Id = 2, TableNumber = 10, Seats = 6 }
            };

            var viewModels = new List<TableIndexViewModel>
            {
                new TableIndexViewModel { Id = 1, TableNumber = 5, Seats = 4 },
                new TableIndexViewModel { Id = 2, TableNumber = 10, Seats = 6 }
            };

            _tableRepo.Setup(r => r.GetAllAsync())
                     .ReturnsAsync(tables);

            _mapper.Setup(m => m.Map<List<TableIndexViewModel>>(tables))
                  .Returns(viewModels);

            // Act
            var result = await _service.GetAllTablesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        #endregion

        #region GetTableDetailsAsync Tests

        [Fact]
        public async Task GetTableDetailsAsync_WithValidId_ReturnsDetails()
        {
            // Arrange
            var table = new Table { Id = 1, TableNumber = 5, Seats = 4 };

            var viewModel = new TableDetailsViewModel
            {
                Id = 1,
                TableNumber = 5,
                Seats = 4
            };

            _tableRepo.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync(table);

            _tableRepo.Setup(r => r.GetTodayReservationCountAsync(1))
                     .ReturnsAsync(2);

            _tableRepo.Setup(r => r.GetTotalReservationCountAsync(1))
                     .ReturnsAsync(10);

            _tableRepo.Setup(r => r.GetFutureReservationCountAsync(1))
                     .ReturnsAsync(5);

            _mapper.Setup(m => m.Map<TableDetailsViewModel>(table))
                  .Returns(viewModel);

            // Act
            var result = await _service.GetTableDetailsAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(2, result.TodayReservationCount);
            Assert.Equal(10, result.TotalReservations);
            Assert.Equal(5, result.FutureReservationCount);
        }

        [Fact]
        public async Task GetTableDetailsAsync_TableNotFound_ReturnsNull()
        {
            // Arrange
            _tableRepo.Setup(r => r.GetByIdAsync(999))
                     .ReturnsAsync((Table?)null);

            // Act
            var result = await _service.GetTableDetailsAsync(999);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region AddTableAsync Tests

        [Fact]
        public async Task AddTableAsync_WithValidData_AddsTableSuccessfully()
        {
            // Arrange
            var model = new TableFormViewModel
            {
                TableNumber = 5,
                Seats = 4
            };

            var table = new Table { TableNumber = 5, Seats = 4 };

            _tableRepo.Setup(r => r.TableNumberExistsAsync(5, null))
                     .ReturnsAsync(false);

            _mapper.Setup(m => m.Map<Table>(model))
                  .Returns(table);

            // Act
            await _service.AddTableAsync(model);

            // Assert
            _tableRepo.Verify(r => r.AddAsync(It.IsAny<Table>()), Times.Once);
            _tableRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task AddTableAsync_TableNumberExists_ThrowsException()
        {
            // Arrange
            var model = new TableFormViewModel
            {
                TableNumber = 5,
                Seats = 4
            };

            _tableRepo.Setup(r => r.TableNumberExistsAsync(5, null))
                     .ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.AddTableAsync(model));

            Assert.Contains("already exists", exception.Message);
            _tableRepo.Verify(r => r.AddAsync(It.IsAny<Table>()), Times.Never);
        }

        #endregion

        #region EditTableAsync Tests

        [Fact]
        public async Task EditTableAsync_WithValidData_UpdatesSuccessfully()
        {
            // Arrange
            var table = new Table { Id = 1, TableNumber = 5, Seats = 4 };

            var model = new TableFormViewModel
            {
                Id = 1,
                TableNumber = 5,
                Seats = 6
            };

            _tableRepo.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync(table);

            _tableRepo.Setup(r => r.TableNumberExistsAsync(5, 1))
                     .ReturnsAsync(false);

            _mapper.Setup(m => m.Map(model, table))
                  .Returns(table);

            // Act
            var result = await _service.EditTableAsync(model);

            // Assert
            Assert.True(result);
            _tableRepo.Verify(r => r.Update(It.IsAny<Table>()), Times.Once);
            _tableRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task EditTableAsync_TableNotFound_ReturnsFalse()
        {
            // Arrange
            _tableRepo.Setup(r => r.GetByIdAsync(999))
                     .ReturnsAsync((Table?)null);

            var model = new TableFormViewModel { Id = 999, TableNumber = 5, Seats = 4 };

            // Act
            var result = await _service.EditTableAsync(model);

            // Assert
            Assert.False(result);
            _tableRepo.Verify(r => r.Update(It.IsAny<Table>()), Times.Never);
        }

        [Fact]
        public async Task EditTableAsync_TableNumberExists_ThrowsException()
        {
            // Arrange
            var table = new Table { Id = 1, TableNumber = 5, Seats = 4 };

            var model = new TableFormViewModel
            {
                Id = 1,
                TableNumber = 10,
                Seats = 4
            };

            _tableRepo.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync(table);

            _tableRepo.Setup(r => r.TableNumberExistsAsync(10, 1))
                     .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.EditTableAsync(model));
        }

        #endregion

        #region DeleteTableAsync Tests

        [Fact]
        public async Task DeleteTableAsync_WithValidId_DeletesSuccessfully()
        {
            // Arrange
            var table = new Table { Id = 1, TableNumber = 5, Seats = 4 };

            _tableRepo.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync(table);

            _tableRepo.Setup(r => r.HasFutureReservationsAsync(1))
                     .ReturnsAsync(false);

            // Act
            var result = await _service.DeleteTableAsync(1);

            // Assert
            Assert.True(result);
            _tableRepo.Verify(r => r.Delete(It.IsAny<Table>()), Times.Once);
            _tableRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteTableAsync_TableNotFound_ReturnsFalse()
        {
            // Arrange
            _tableRepo.Setup(r => r.GetByIdAsync(999))
                     .ReturnsAsync((Table?)null);

            // Act
            var result = await _service.DeleteTableAsync(999);

            // Assert
            Assert.False(result);
            _tableRepo.Verify(r => r.Delete(It.IsAny<Table>()), Times.Never);
        }

        [Fact]
        public async Task DeleteTableAsync_HasFutureReservations_ThrowsException()
        {
            // Arrange
            var table = new Table { Id = 1, TableNumber = 5, Seats = 4 };

            _tableRepo.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync(table);

            _tableRepo.Setup(r => r.HasFutureReservationsAsync(1))
                     .ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.DeleteTableAsync(1));

            Assert.Contains("reservation history", exception.Message);
            _tableRepo.Verify(r => r.Delete(It.IsAny<Table>()), Times.Never);
        }

        #endregion
    }
}