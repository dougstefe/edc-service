
using Edc.Core.SharedContext.UseCases;
using MediatR;

namespace Edc.Core.AccountContext.UseCases.Create;

public class Request : IRequest<Response<ResponseData>> {
    public Request(string name, string email, string password)
    {
        Name = name;
        Email = email;
        Password = password;
    }

    public string Name {get;}
    public string Email {get;}
    public string Password {get;}
}