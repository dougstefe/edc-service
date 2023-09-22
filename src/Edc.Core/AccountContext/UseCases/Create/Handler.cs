using Edc.Core.AccountContext.Entities;
using Edc.Core.AccountContext.UseCases.Create.Interfaces;
using Edc.Core.Notifications;
using Edc.Core.SharedContext.UseCases;
using Edc.Core.SharedContext.ValueObjects;
using MediatR;

namespace Edc.Core.AccountContext.UseCases.Create;

public class Handler : SharedContext.UseCases.Handler, IRequestHandler<Request, Response<ResponseData>> {
    private readonly IAddRepository _addRepository;
    private readonly IExistsRepository _existsRepository;
    private readonly IGetUserRoleRepository _getUserRoleRepository;
    private readonly IService _service;
    private readonly IValidation _validation;
    private readonly Notification _notification;

    public Handler(IAddRepository addRepository, IExistsRepository existsRepository, IGetUserRoleRepository getUserRoleRepository, IService service, IValidation validation, Notification notification)
    {
        _addRepository = addRepository;
        _existsRepository = existsRepository;
        _getUserRoleRepository = getUserRoleRepository;
        _service = service;
        _validation = validation;
        _notification = notification;
    }

    public async Task<Response<ResponseData>> Handle(Request request, CancellationToken cancellationToken) {
        var isValid = _validation.Validate(request);
        if(!isValid){
            return new Response<ResponseData>();
        }


        Account account;
        try {
            var name = new Name(request.Name);
            var email = new Email(request.Email);
            var password = new Password(request.Password);
            
            var userRole = await _getUserRoleRepository.GetUserRoleAsync(cancellationToken);

            account = new Account(name, email, password, userRole);
        }
        catch (Exception ex) {
            var (type, error) = GenerateResponseError(ex);
            _notification.AddNotificationMessage(error, type);
            return new Response<ResponseData>();
        }

        var exists = await _existsRepository.ExistsAsync(account.Email.Address, cancellationToken);
        
        if (exists) {
            _notification.AddNotificationMessage("Account already exists.");
            return new Response<ResponseData>();
        }

        await _addRepository.AddAsync(account, cancellationToken);

        await _service.SendVerificationAsync(account, cancellationToken);

        return new Response<ResponseData>(new ResponseData(account.Id));
    }

    
}