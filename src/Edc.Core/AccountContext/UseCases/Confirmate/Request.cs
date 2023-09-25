
using Edc.Core.SharedContext.UseCases;
using MediatR;

namespace Edc.Core.AccountContext.UseCases.Confirmate;

public class Request : IRequest<Response<ResponseData>> {
    public Request(Guid id, string code)
    {
        Id = id;
        Code = code;
    }

    public Guid Id { get; }
    public string Code { get; }
}
