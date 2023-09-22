using Edc.Core;
using Edc.Core.AccountContext.Entities;
using Edc.Core.AccountContext.UseCases.Create.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Edc.Infra.AccountContext.UseCases.Create;

public class Service : IService
{
    private readonly SendGridClient _client;
    private readonly EmailAddress _from;
    private const string SUBJECT = "Verificação de conta";
    private const string CONTENT = "Código de verificação: {0}";

    public Service()
    {
        _client = new SendGridClient(Config.SendGridClient.ApiKey);
        _from = new EmailAddress(Config.Email.DefaultFromEmail, Config.Email.DefaultFromName);
        
    }

    public async Task SendVerificationAsync(Account account, CancellationToken cancellationToken)
    {
        var to = new EmailAddress(account.Email, account.Name);
        var plainTextContent = string.Format(CONTENT, account.Email.VerificationCode);
        var htmlContent = string.Format(CONTENT, $"<strong>{account.Email.VerificationCode.Code}</strong>");
        var email = MailHelper.CreateSingleEmail(
            from: _from,
            to: to,
            subject: SUBJECT,
            plainTextContent: plainTextContent,
            htmlContent: htmlContent
        );
        await _client.SendEmailAsync(email, cancellationToken);
    }
}