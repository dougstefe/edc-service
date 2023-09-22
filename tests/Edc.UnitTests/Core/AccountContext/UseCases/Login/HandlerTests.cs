using Bogus;
using Edc.Core.AccountContext.Entities;
using Edc.Core.AccountContext.UseCases.Login;
using Edc.Core.AccountContext.UseCases.Login.Interfaces;
using Edc.Core.Notifications;
using Edc.Core.SharedContext.ValueObjects;
using Edc.UnitTests.Core.AccountContext.Entities;
using Moq;
using Xunit;

namespace Edc.UnitTests.Core.AccountContext.UseCases.Login;

public class HandlerTests {
    private readonly Mock<IGetRepository> _getRepositoryMock;
    private readonly IValidation _validation;
    private readonly Mock<Notification> _notificationMock;
    private readonly Handler _sut;

    public HandlerTests()
    {
        _getRepositoryMock = new Mock<IGetRepository>();
        _notificationMock = new Mock<Notification>();
        _validation = new Validation(_notificationMock.Object);
        _sut = new Handler(_getRepositoryMock.Object, _validation, _notificationMock.Object);
    }

    [Fact]
    public async Task ShouldAddNotificationWhenRequestEmailIsInvalid() {
        var invalidEmail = "";
        var password = "@nyP@55w0rd";
        var request = new Request(invalidEmail, password);

        await _sut.Handle(request, new CancellationToken());

        Assert.NotEmpty(_notificationMock.Object.NotificationMessages);
    }

    [Fact]
    public async Task ShouldAddNotificationWhenRequestPasswordIsInvalid() {
        var email = "any@email.com";
        var invalidPassword = "";
        var request = new Request(email, invalidPassword);

        await _sut.Handle(request, new CancellationToken());

        Assert.NotEmpty(_notificationMock.Object.NotificationMessages);
    }

    [Fact]
    public async Task ShouldCallGetRepository() {
        var email = "any@email.com";
        var password = "@nyP@55w0rd";
        var request = new Request(email, password);
        var cancellationToken = new CancellationToken();

        await _sut.Handle(request, cancellationToken);

        _getRepositoryMock.Verify(x => x.GetAsync(email, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task ShouldAddNotificationWhenAccountNotFound() {
        var email = "any@email.com";
        var password = "@nyP@55w0rd";
        var request = new Request(email, password);
        var cancellationToken = new CancellationToken();

        _getRepositoryMock.Setup(x => x.GetAsync(email, cancellationToken)).ReturnsAsync(null as Account);

        await _sut.Handle(request, cancellationToken);
        
        Assert.NotEmpty(_notificationMock.Object.NotificationMessages);
    }

    [Fact]
    public async Task ShouldAddNotificationWhenVerificationCodeIsNotActive() {
        var email = "any@email.com";
        var password = "@nyP@55w0rd";
        var request = new Request(email, password);
        var cancellationToken = new CancellationToken();
        var accountFake = new Faker<Account>()
            .CustomInstantiator(x =>
                new Account(
                    x.Person.FullName,
                    new Email(email),
                    new Password(password),
                    RoleMock.RoleFaker.Generate()
                )
            ).Generate();

        _getRepositoryMock.Setup(x => x.GetAsync(email, cancellationToken)).ReturnsAsync(accountFake);

        await _sut.Handle(request, cancellationToken);
        
        Assert.NotEmpty(_notificationMock.Object.NotificationMessages.Where(x => x.Message.Equals("This account is not activated.")));
    }

    [Fact]
    public async Task ShouldReturnValidResponseWhenResponseIsSuccess() {
        var email = "any@email.com";
        var password = "@nyP@55w0rd";
        var request = new Request(email, password);
        var cancellationToken = new CancellationToken();
        var emailFake = new Email(email);
        var accountFake = new Faker<Account>()
            .CustomInstantiator(x =>
                new Account(
                    x.Person.FullName,
                    emailFake,
                    new Password(password),
                    RoleMock.RoleFaker.Generate()
                )
            ).Generate();
        var code = emailFake.VerificationCode.Code;
        accountFake.Email.VerificationCode.Verify(code);

        _getRepositoryMock.Setup(x => x.GetAsync(email, cancellationToken)).ReturnsAsync(accountFake);

        var response = await _sut.Handle(request, cancellationToken);
        
        Assert.True(response.IsSuccess);
        Assert.NotNull(response.Data);
        Assert.Equal(accountFake.Id, response.Data.Id);
        Assert.Equal(accountFake.Name, response.Data.Name);
        Assert.Equal(accountFake.Email, response.Data.Email);
        Assert.Equal(accountFake.Roles.Select(x => x.Name), response.Data.Roles);
    }
}
