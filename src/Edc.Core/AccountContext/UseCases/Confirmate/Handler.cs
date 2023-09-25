using Edc.Core.AccountContext.UseCases.Confirmate.Interfaces;
using Edc.Core.Exceptions;
using Edc.Core.Notifications;
using Edc.Core.SharedContext.UseCases;
using MediatR;

namespace Edc.Core.AccountContext.UseCases.Confirmate;

public class Handler : SharedContext.UseCases.Handler, IRequestHandler<Request, Response<ResponseData>> {
    private readonly IUpdateRepository _updateRepository;
    private readonly IGetByIdRepository _getByIdRepository;
    private readonly IValidation _validation;
    private readonly Notification _notification;

    public Handler(IUpdateRepository updateRepository, IGetByIdRepository getByIdRepository, IValidation validation, Notification notification)
    {
        _updateRepository = updateRepository;
        _getByIdRepository = getByIdRepository;
        _validation = validation;
        _notification = notification;
    }

    public async Task<Response<ResponseData>> Handle(Request request, CancellationToken cancellationToken) {
        var isValid = _validation.Validate(request);
        if(!isValid){
            return new Response<ResponseData>();
        }
        var account = await _getByIdRepository.GetByIdAsync(request.Id, cancellationToken);

        try {
            account.Email.VerificationCode.Verify(request.Code);

            await _updateRepository.UpdateAsync(account, cancellationToken);
        }
        catch (Exception ex) {
            var (type, error) = GenerateResponseError(ex);
            _notification.AddNotificationMessage(error, type);
            return new Response<ResponseData>();
        }
        
        var responseData = new ResponseData(account.Id, account.Name, account.Email, account.Roles.Select(x => x.Name).ToArray());
        return new Response<ResponseData>(responseData);
    }
}
