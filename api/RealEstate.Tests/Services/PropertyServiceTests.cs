using NUnit.Framework;
using Moq;
using RealEstate.Application.Services;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Dto;

namespace RealEstate.Tests
{
    [TestFixture]
    public class PropertyServiceTests
    {
        private Mock<IPropertyRepository> _propertyRepoMock;
        private Mock<IOwnerRepository> _ownerRepoMock;
        private PropertyService _service;

        [SetUp]
        public void Setup()
        {
            _propertyRepoMock = new Mock<IPropertyRepository>();
            _ownerRepoMock = new Mock<IOwnerRepository>();
            _service = new PropertyService(_propertyRepoMock.Object, _ownerRepoMock.Object);
        }

        [Test]
        public async Task GetFilteredPagedAsync_ShouldReturnPropertiesWithOwners()
        {
            // Arrange
            var properties = new List<Property>
            {
                new Property { Id = "1", Name = "Casa", Address = "Calle 123", Price = 100000, IdOwner = "owner1", ImageUrl = "img1" },
                new Property { Id = "2", Name = "Apartamento", Address = "Avenida 456", Price = 200000, IdOwner = "owner2", ImageUrl = "img2" }
            };

            _propertyRepoMock
                .Setup(r => r.GetFilteredPagedAsync(null, null, null, null, 1, 10))
                .ReturnsAsync((properties, 2));

            _ownerRepoMock
                .Setup(r => r.GetByIdAsync("owner1"))
                .ReturnsAsync(new Owner { Id = "owner1", Name = "Alicia", Email = "alicia@ejemplo.com" });

            _ownerRepoMock
                .Setup(r => r.GetByIdAsync("owner2"))
                .ReturnsAsync(new Owner { Id = "owner2", Name = "Roberto", Email = "roberto@ejemplo.com" });

            // Act
            var result = await _service.GetFilteredPagedAsync(null, null, null, null, 1, 10);

            // Assert
            Assert.That(result.Items.Count, Is.EqualTo(2));
            Assert.That(result.Items[0].Owner!.Name, Is.EqualTo("Alicia"));
            Assert.That(result.Items[1].Owner!.Name, Is.EqualTo("Roberto"));
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnDtoWithOwner_WhenFound()
        {
            var property = new Property
            {
                Id = "1",
                Name = "Casa",
                Address = "Calle Principal",
                Price = 123,
                IdOwner = "owner1",
                ImageUrl = "imagen"
            };
            var owner = new Owner
            {
                Id = "owner1",
                Name = "Juan",
                Email = "juan@ejemplo.com"
            };

            _propertyRepoMock.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(property);
            _ownerRepoMock.Setup(r => r.GetByIdAsync("owner1")).ReturnsAsync(owner);

            var result = await _service.GetByIdAsync("1");

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Name, Is.EqualTo("Casa"));
            Assert.That(result.Owner!.Email, Is.EqualTo("juan@ejemplo.com"));
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            _propertyRepoMock.Setup(r => r.GetByIdAsync("404")).ReturnsAsync((Property?)null);

            var result = await _service.GetByIdAsync("404");

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task CreateAsync_ShouldCallRepositoryWithMappedEntity()
        {
            var dto = new PropertyDto
            {
                Name = "Nueva Propiedad",
                Address = "Dirección 1",
                Price = 500,
                IdOwner = "owner1",
                ImageUrl = "imagen.jpg"
            };

            await _service.CreateAsync(dto);

            _propertyRepoMock.Verify(r => r.CreateAsync(It.Is<Property>(
                p => p.Name == "Nueva Propiedad" && p.Address == "Dirección 1" && p.IdOwner == "owner1"
            )), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateEntity_WhenExists()
        {
            var existing = new Property
            {
                Id = "1",
                Name = "Nombre Antiguo",
                Address = "Dirección Antiguo",
                Price = 100,
                IdOwner = "owner1",
                ImageUrl = "img_vieja.jpg"
            };

            _propertyRepoMock.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(existing);

            var dto = new PropertyDto
            {
                Name = "Nombre Nuevo",
                Address = "Dirección Nueva",
                Price = 999,
                IdOwner = "owner2",
                ImageUrl = "img_nueva.jpg"
            };

            await _service.UpdateAsync("1", dto);

            _propertyRepoMock.Verify(r => r.UpdateAsync(It.Is<Property>(
                p => p.Name == "Nombre Nuevo" && p.Address == "Dirección Nueva" &&
                     p.Price == 999 && p.ImageUrl == "img_nueva.jpg" && p.IdOwner == "owner2"
            )), Times.Once);
        }

        [Test]
        public void UpdateAsync_ShouldThrow_WhenPropertyNotFound()
        {
            _propertyRepoMock.Setup(r => r.GetByIdAsync("404")).ReturnsAsync((Property?)null);

            var dto = new PropertyDto { Name = "Propiedad", Address = "Dirección", Price = 1, IdOwner = "owner1", ImageUrl = "img.jpg" };

            var ex = Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync("404", dto));
            Assert.That(ex!.Message, Is.EqualTo("Property not found"));
        }

        [Test]
        public async Task DeleteAsync_ShouldCallRepository()
        {
            await _service.DeleteAsync("1");

            _propertyRepoMock.Verify(r => r.DeleteAsync("1"), Times.Once);
        }
    }
}
