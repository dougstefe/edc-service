
using Edc.Core.SharedContext.UseCases;
using MediatR;

namespace Edc.Core.AccountContext.UseCases.Login;

public class Request : IRequest<Response<ResponseData>>
{
    public Request(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public string Email { get; }
    public string Password { get; }
}
