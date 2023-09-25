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
}
