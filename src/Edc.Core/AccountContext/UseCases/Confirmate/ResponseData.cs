

namespace Edc.Core.AccountContext.UseCases.Confirmate;

public record ResponseData(Guid Id, string Name, string Email, string[] Roles);