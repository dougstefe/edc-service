namespace Edc.Core.AccountContext.UseCases.Login;

public record ResponseData(Guid Id, string Name, string Email, string[] Roles);