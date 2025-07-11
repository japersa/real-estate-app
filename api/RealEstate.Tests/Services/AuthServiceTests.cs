using Moq;
using Microsoft.Extensions.Configuration;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Dto;
using RealEstate.Infrastructure.Services;

namespace RealEstate.Tests.Services
{
    [TestFixture]
    public class AuthServiceTests
    {
        private Mock<IUserRepository> _userRepoMock = null!;
        private Mock<IConfiguration> _configMock = null!;
        private AuthService _authService = null!;

        [SetUp]
        public void Setup()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _configMock = new Mock<IConfiguration>();

            _configMock.Setup(c => c["Jwt:Key"]).Returns("esta_es_una_clave_secreta_super_segura_123456789!");
            _configMock.Setup(c => c["Jwt:Issuer"]).Returns("realestate-api");
            _configMock.Setup(c => c["Jwt:Audience"]).Returns("realestate-client");

            _authService = new AuthService(_configMock.Object, _userRepoMock.Object);
        }

        [Test]
        public async Task LoginAsync_CredencialesValidas_RetornaToken()
        {
            var email = "usuario@ejemplo.com";
            var password = "Test1234";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = email,
                PasswordHash = hashedPassword
            };

            _userRepoMock.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync(user);

            var request = new AuthRequest { Email = email, Password = password };

            var result = await _authService.LoginAsync(request);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Token, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public async Task LoginAsync_ContraseñaInvalida_RetornaNull()
        {
            var email = "usuario@ejemplo.com";

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("ContraseñaCorrecta")
            };

            _userRepoMock.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync(user);

            var request = new AuthRequest { Email = email, Password = "ContraseñaIncorrecta" };

            var result = await _authService.LoginAsync(request);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task LoginAsync_UsuarioNoExiste_RetornaNull()
        {
            _userRepoMock.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((User?)null);

            var request = new AuthRequest { Email = "noexiste@ejemplo.com", Password = "cualquiercosa" };

            var result = await _authService.LoginAsync(request);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task RegisterAsync_UsuarioNuevo_RetornaTrue()
        {
            var email = "nuevo@ejemplo.com";
            var password = "Test1234";

            _userRepoMock.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync((User?)null);
            _userRepoMock.Setup(r => r.CreateAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            var result = await _authService.RegisterAsync(new AuthRequest { Email = email, Password = password });

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task RegisterAsync_EmailExistente_RetornaFalse()
        {
            var email = "existente@ejemplo.com";

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = email,
                PasswordHash = "hash"
            };

            _userRepoMock.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync(user);

            var result = await _authService.RegisterAsync(new AuthRequest { Email = email, Password = "cualquier" });

            Assert.That(result, Is.False);
        }
    }
}
