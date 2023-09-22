using Edc.Core.AccountContext.UseCases.Login.Interfaces;
using Edc.Core.Notifications;
using Edc.Core.SharedContext.UseCases;
using Edc.Core.SharedContext.ValueObjects;
using MediatR;

namespace Edc.Core.AccountContext.UseCases.Login;

public class Handler : SharedContext.UseCases.Handler, IRequestHandler<Request, Response<ResponseData>>
{
    private readonly IGetRepository _getRepository;
    private readonly IValidation _validation;
    private readonly Notification _notification;

    public Handler(IGetRepository getRepository, IValidation validation, Notification notification)
    {
        _getRepository = getRepository;
        _validation = validation;
        _notification = notification;
    }

    public async Task<Response<ResponseData>> Handle(Request request, CancellationToken cancellationToken)
    {
        var isValid = _validation.Validate(request);
        if(!isValid){
            return new Response<ResponseData>();
        }

        try {
            var email = new Email(request.Email);

            var account = await _getRepository.GetAsync(email, cancellationToken);

            if (account is null || !account.Password.Compare(request.Password)) {
                _notification.AddNotificationMessage($"Invalid {nameof(request.Email)} or {nameof(request.Password)}.");
                return new Response<ResponseData>();
            }

            if (!account.Email.VerificationCode.IsActive) {
                _notification.AddNotificationMessage("This account is not activated.");
                return new Response<ResponseData>();
            }

            var responseData = new ResponseData(account.Id, account.Name, account.Email, account.Roles.Select(x => x.Name).ToArray());
            return new Response<ResponseData>(responseData);
        }
        catch (Exception ex) {
            var (type, error) = GenerateResponseError(ex);
            _notification.AddNotificationMessage(error, type);
            return new Response<ResponseData>();
        }
    }

    
}