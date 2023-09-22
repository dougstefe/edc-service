using Edc.Core.AccountContext.Entities;
using Edc.Core.AccountContext.UseCases.Create;
using Edc.Core.AccountContext.UseCases.Create.Interfaces;
using Edc.Core.Notifications;
using Edc.Core.SharedContext.ValueObjects;
using Edc.UnitTests.Core.AccountContext.Entities;
using Moq;
using Xunit;

namespace Edc.UnitTests.Core.AccountContext.UseCases.Create;

public class HandlerTests {
    private readonly Mock<IAddRepository> _addRepositoryMock;
    private readonly Mock<IExistsRepository> _existsRepositoryMock;
    private readonly Mock<IGetUserRoleRepository> _getUserRoleRepositoryMock;
    private readonly Mock<IService> _serviceMock;
    private readonly IValidation _validation;
    private readonly Mock<Notification> _notificationMock;
    private Handler _sut;

    public HandlerTests()
    {
        _addRepositoryMock = new Mock<IAddRepository>();
        _existsRepositoryMock = new Mock<IExistsRepository>();
        _getUserRoleRepositoryMock = new Mock<IGetUserRoleRepository>();
        _serviceMock = new Mock<IService>();
        _notificationMock = new Mock<Notification>();
        _validation = new Validation(_notificationMock.Object);
        _sut = new Handler(
            _addRepositoryMock.Object,
            _existsRepositoryMock.Object,
            _getUserRoleRepositoryMock.Object,
            _serviceMock.Object,
            _validation,
            _notificationMock.Object
        );
    }
        
    [Fact]
    public async Task ShouldAddNotificationWhenRequestNameIsInvalid() {
        var invalidName = "";
        var email = "any@mail.com";
        var password = "@nyP@55w0rd";
        var request = new Request(invalidName, email, password);

        await _sut.Handle(request, new CancellationToken());

        Assert.NotEmpty(_notificationMock.Object.NotificationMessages);
    }
        
    [Fact]
    public async Task ShouldAddNotificationWhenRequestEmailIsInvalid() {
        var name = "Any Name";
        var invalidEmail = "any@mail";
        var password = "@nyP@55w0rd";
        var request = new Request(name, invalidEmail, password);

        await _sut.Handle(request, new CancellationToken());

        Assert.NotEmpty(_notificationMock.Object.NotificationMessages);
    }
        
    [Fact]
    public async Task ShouldAddNotificationWhenRequestPasswordIsInvalid() {
        var name = "Any Name";
        var email = "any@mail.com";
        var invalidPassword = "@nyP@55";
        var request = new Request(name, email, invalidPassword);

        await _sut.Handle(request, new CancellationToken());

        Assert.NotEmpty(_notificationMock.Object.NotificationMessages);
    }
        
    [Fact]
    public async Task ShouldCallGetUserRoleRepository() {
        var name = "Any Name";
        var email = "any@mail.com";
        var password = "@nyP@55w0rd";
        var request = new Request(name, email, password);

        await _sut.Handle(request, new CancellationToken());

        _getUserRoleRepositoryMock.Verify(x => x.GetUserRoleAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
        
    [Fact]
    public async Task ShouldCallExistsRepositoryWithCorrectValue() {
        var roleFake = RoleMock.RoleFaker.Generate();
        _getUserRoleRepositoryMock.Setup(x => x.GetUserRoleAsync(It.IsAny<CancellationToken>())).ReturnsAsync(roleFake);

        var name = "Any Name";
        var email = "any@mail.com";
        var password = "@nyP@55w0rd";
        var request = new Request(name, email, password);
        var cancellationToken = new CancellationToken();

        await _sut.Handle(request, cancellationToken);

        _existsRepositoryMock.Verify(x => x.ExistsAsync(email, cancellationToken), Times.Once);
    }
        
    [Fact]
    public async Task ShouldAddNotificationWhenAccountAlreadyExists() {
        var name = "Any Name";
        var email = "any@mail.com";
        var password = "@nyP@55w0rd";
        var roleFake = RoleMock.RoleFaker.Generate();
        var cancellationToken = new CancellationToken();

        _getUserRoleRepositoryMock.Setup(x => x.GetUserRoleAsync(It.IsAny<CancellationToken>())).ReturnsAsync(roleFake);
        _existsRepositoryMock.Setup(x => x.ExistsAsync(email, cancellationToken)).ReturnsAsync(true);

        var request = new Request(name, email, password);

        await _sut.Handle(request, cancellationToken);

        Assert.NotEmpty(_notificationMock.Object.NotificationMessages);
    }
        
    [Fact]
    public async Task ShouldCallAddRepository() {
        var name = "Any Name";
        var email = "any@mail.com";
        var password = "@nyP@55w0rd";
        var roleFake = RoleMock.RoleFaker.Generate();
        var cancellationToken = new CancellationToken();

        _getUserRoleRepositoryMock.Setup(x => x.GetUserRoleAsync(It.IsAny<CancellationToken>())).ReturnsAsync(roleFake);
        _existsRepositoryMock.Setup(x => x.ExistsAsync(email, cancellationToken)).ReturnsAsync(false);

        var request = new Request(name, email, password);

        await _sut.Handle(request, cancellationToken);

        _addRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Account>(), cancellationToken), Times.Once);
    }
        
    [Fact]
    public async Task ShouldCallSendVerificationService() {
        var name = "Any Name";
        var email = "any@mail.com";
        var password = "@nyP@55w0rd";
        var roleFake = RoleMock.RoleFaker.Generate();
        var cancellationToken = new CancellationToken();

        _getUserRoleRepositoryMock.Setup(x => x.GetUserRoleAsync(It.IsAny<CancellationToken>())).ReturnsAsync(roleFake);
        _existsRepositoryMock.Setup(x => x.ExistsAsync(email, cancellationToken)).ReturnsAsync(false);

        var request = new Request(name, email, password);

        await _sut.Handle(request, cancellationToken);

        _serviceMock.Verify(x => x.SendVerificationAsync(It.IsAny<Account>(), cancellationToken), Times.Once);
    }
        
    [Fact]
    public async Task ShouldReturnResponseIsSuccessTrue() {
        var name = "Any Name";
        var email = "any@mail.com";
        var password = "@nyP@55w0rd";
        var roleFake = RoleMock.RoleFaker.Generate();
        var cancellationToken = new CancellationToken();

        _getUserRoleRepositoryMock.Setup(x => x.GetUserRoleAsync(It.IsAny<CancellationToken>())).ReturnsAsync(roleFake);
        _existsRepositoryMock.Setup(x => x.ExistsAsync(email, cancellationToken)).ReturnsAsync(false);

        var request = new Request(name, email, password);

        var response = await _sut.Handle(request, cancellationToken);

        Assert.True(response.IsSuccess);
    }
        
    [Fact]
    public async Task ShouldReturnResponseData() {
        var name = "Any Name";
        var email = "any@mail.com";
        var password = "@nyP@55w0rd";
        var roleFake = RoleMock.RoleFaker.Generate();
        var cancellationToken = new CancellationToken();

        _getUserRoleRepositoryMock.Setup(x => x.GetUserRoleAsync(It.IsAny<CancellationToken>())).ReturnsAsync(roleFake);
        _existsRepositoryMock.Setup(x => x.ExistsAsync(email, cancellationToken)).ReturnsAsync(false);

        var request = new Request(name, email, password);

        var response = await _sut.Handle(request, cancellationToken);

        Assert.NotNull(response.Data);
    }
}