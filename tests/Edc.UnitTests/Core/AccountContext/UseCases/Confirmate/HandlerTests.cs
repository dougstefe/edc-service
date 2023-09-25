using Bogus;
using Edc.Core.AccountContext.Entities;
using Edc.Core.AccountContext.UseCases.Confirmate;
using Edc.Core.AccountContext.UseCases.Confirmate.Interfaces;
using Edc.Core.Notifications;
using Edc.Core.SharedContext.ValueObjects;
using Edc.UnitTests.Core.AccountContext.Entities;
using Moq;
using Xunit;

namespace Edc.UnitTests.Core.AccountContext.UseCases.Confirmate;

public class HandlerTests {
    private readonly Mock<IUpdateRepository> _updateRepositoryMock;
    private readonly Mock<IGetByIdRepository> _getByIdRepositoryMock;
    private readonly IValidation _validation;
    private readonly Mock<Notification> _notificationMock;
    private readonly Handler _sut;

    public HandlerTests()
    {
        _updateRepositoryMock = new();
        _getByIdRepositoryMock = new();
        _notificationMock = new();
        _validation = new Validation(_notificationMock.Object);
        _sut = new(_updateRepositoryMock.Object, _getByIdRepositoryMock.Object, _validation, _notificationMock.Object);
    }

    [Fact]
    public async Task ShouldAddNotificationWhenRequestIdIsInvalid() {
        var invalidId = Guid.Empty;
        var code = "any_code";
        var request = new Request(invalidId, code);

        await _sut.Handle(request, new CancellationToken());

        Assert.NotEmpty(_notificationMock.Object.NotificationMessages.Where(x => x.Message.Equals($"{nameof(Request.Id)} is required.")));
    }

    [Fact]
    public async Task ShouldAddNotificationWhenRequestCodeIsInvalid() {
        var id = Guid.NewGuid();
        var invalidCode = "";
        var request = new Request(id, invalidCode);

        await _sut.Handle(request, new CancellationToken());

        Assert.NotEmpty(_notificationMock.Object.NotificationMessages.Where(x => x.Message.Equals($"{nameof(Request.Code)} is required.")));
    }

    [Fact]
    public async Task ShouldCallGetByIdRepository() {
        var id = Guid.NewGuid();
        var code = "any_code";
        var request = new Request(id, code);
        var cancellationToken = new CancellationToken();

        await _sut.Handle(request, cancellationToken);

        _getByIdRepositoryMock.Verify(x => x.GetByIdAsync(request.Id, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task ShouldAddNotificationWhenVerifyCodeThrows() {

        var id = Guid.NewGuid();
        var code = "any_code";
        var request = new Request(id, code);
        var cancellationToken = new CancellationToken();
        var accountFake = AccountMock.AccountFaker.Generate();
        
        _getByIdRepositoryMock.Setup(x => x.GetByIdAsync(request.Id, cancellationToken)).ReturnsAsync(accountFake);

        await _sut.Handle(request, cancellationToken);

        Assert.NotEmpty(_notificationMock.Object.NotificationMessages.Where(x => x.Message.Equals("This code is invalid.")));
    }

    [Fact]
    public async Task ShouldAddNotificationWhenUpdateRepositoryThrows() {
        
        var emailFake = new Email("any@emai.com");
        var accountFake = new Faker<Account>()
            .CustomInstantiator(x =>
                new Account(
                    x.Person.FullName,
                    emailFake,
                    new Password("@ny_p@55w0rd"),
                    RoleMock.RoleFaker.Generate()
                )
            ).Generate();

        var code = accountFake.Email.VerificationCode.Code;
        var request = new Request(accountFake.Id, code);
        var cancellationToken = new CancellationToken();

        _getByIdRepositoryMock.Setup(x => x.GetByIdAsync(request.Id, cancellationToken)).ReturnsAsync(accountFake);
        _updateRepositoryMock.Setup(x => x.UpdateAsync(accountFake, cancellationToken)).ThrowsAsync(new Exception("Any Exception"));

        await _sut.Handle(request, cancellationToken);

        Assert.NotEmpty(_notificationMock.Object.NotificationMessages.Where(x => x.Message.Equals("Any Exception")));
    }

    [Fact]
    public async Task ShouldCallUpdateRepositoryWithTheActiveAccount() {
        
        var emailFake = new Email("any@emai.com");
        var accountFake = new Faker<Account>()
            .CustomInstantiator(x =>
                new Account(
                    x.Person.FullName,
                    emailFake,
                    new Password("@ny_p@55w0rd"),
                    RoleMock.RoleFaker.Generate()
                )
            ).Generate();

        var code = accountFake.Email.VerificationCode.Code;
        var request = new Request(accountFake.Id, code);
        var cancellationToken = new CancellationToken();

        _getByIdRepositoryMock.Setup(x => x.GetByIdAsync(request.Id, cancellationToken)).ReturnsAsync(accountFake);

        await _sut.Handle(request, cancellationToken);

        _updateRepositoryMock.Verify(x => x.UpdateAsync(It.Is<Account>(x => x.Id == request.Id && x.Email.VerificationCode.IsActive), cancellationToken), Times.Once);
    }
}
