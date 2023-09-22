using Edc.Core.AccountContext.UseCases.Login.Interfaces;
using Edc.Core.Notifications;
using FluentValidation;

namespace Edc.Core.AccountContext.UseCases.Login;

public class Validation : AbstractValidator<Request>, IValidation
{
    private readonly Notification _notification;
    public Validation(Notification notification)
    {
        _notification = notification;
        RuleFor(x => x.Email)
                    .NotEmpty()
                    .WithMessage($"{nameof(Request.Email)} is required.")
                    .EmailAddress()
                    .WithMessage($"{nameof(Request.Email)} is invalid.");

        RuleFor(m => m.Password)
                    .NotEmpty()
                    .WithMessage($"{nameof(Request.Password)} is required.")
                    .MinimumLength(8)
                    .WithMessage($"{nameof(Request.Password)} must have at least eight characters.")
                    .MaximumLength(20)
                    .WithMessage($"{nameof(Request.Password)} must have a maximum of eight characters.");
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