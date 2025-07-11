using Moq;
using RealEstate.Application.Services;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Dto;
using RealEstate.Domain.Entities;

namespace RealEstate.Tests
{
    [TestFixture]
    public class OwnerServiceTests
    {
        private Mock<IOwnerRepository> _repoMock;
        private OwnerService _service;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<IOwnerRepository>();
            _service = new OwnerService(_repoMock.Object);
        }

        [Test]
        public async Task GetPagedAsync_ShouldReturnMappedDtoList()
        {
            var owners = new List<Owner>
            {
                new Owner { Id = "1", Name = "Juan Pérez", Email = "juan@ejemplo.com" },
                new Owner { Id = "2", Name = "Ana Gómez", Email = "ana@ejemplo.com" }
            };
            var paged = new PaginatedList<Owner> { Items = owners, TotalCount = 2 };
            _repoMock.Setup(r => r.GetPagedAsync(1, 10)).ReturnsAsync(paged);

            var result = await _service.GetPagedAsync(1, 10);

            Assert.That(result.Items.Count, Is.EqualTo(2));
            Assert.That(result.TotalCount, Is.EqualTo(2));
            Assert.That(result.Items.First().Name, Is.EqualTo("Juan Pérez"));
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnDto_WhenOwnerExists()
        {
            var owner = new Owner { Id = "1", Name = "Pedro", Email = "pedro@ejemplo.com" };
            _repoMock.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(owner);

            var result = await _service.GetByIdAsync("1");

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Name, Is.EqualTo("Pedro"));
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnNull_WhenOwnerNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync("notfound")).ReturnsAsync((Owner?)null);

            var result = await _service.GetByIdAsync("notfound");

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task CreateAsync_ShouldCallRepositoryWithMappedEntity()
        {
            var dto = new OwnerDto { Name = "Lucía", Email = "lucia@ejemplo.com" };

            await _service.CreateAsync(dto);

            _repoMock.Verify(r => r.CreateAsync(It.Is<Owner>(
                o => o.Name == "Lucía" && o.Email == "lucia@ejemplo.com"
            )), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateOwner_WhenExists()
        {
            var existing = new Owner { Id = "1", Name = "Antiguo", Email = "antiguo@ejemplo.com" };
            var dto = new OwnerDto { Name = "Nuevo", Email = "nuevo@ejemplo.com" };

            _repoMock.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(existing);

            await _service.UpdateAsync("1", dto);

            _repoMock.Verify(r => r.UpdateAsync(It.Is<Owner>(
                o => o.Name == "Nuevo" && o.Email == "nuevo@ejemplo.com"
            )), Times.Once);
        }

        [Test]
        public void UpdateAsync_ShouldThrowException_WhenOwnerNotFound()
        {
            var dto = new OwnerDto { Name = "Nuevo", Email = "nuevo@ejemplo.com" };
            _repoMock.Setup(r => r.GetByIdAsync("notfound")).ReturnsAsync((Owner?)null);

            var ex = Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync("notfound", dto));
            Assert.That(ex!.Message, Is.EqualTo("Owner no encontrado"));
        }

        [Test]
        public async Task DeleteAsync_ShouldCallRepository()
        {
            await _service.DeleteAsync("123");

            _repoMock.Verify(r => r.DeleteAsync("123"), Times.Once);
        }
    }
}
