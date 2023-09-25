
using Edc.Core.AccountContext.UseCases.Confirmate.Interfaces;
using Edc.Core.Notifications;
using FluentValidation;

namespace Edc.Core.AccountContext.UseCases.Confirmate;

public class Validation : AbstractValidator<Request>, IValidation {
    
    private readonly Notification _notification;

    public Validation(Notification notification){
        _notification = notification;

        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage($"{nameof(Request.Id)} is required.");

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage($"{nameof(Request.Code)} is required.");
    }

    public bool Validate(Request request)
    {
        var validation = base.Validate(request);
        if(validation.IsValid)
            return true;
        
        foreach (var error in validation.Errors) {
            _notification.AddNotificationMessage(error.ErrorMessage);
        }

        return false;
    }
}