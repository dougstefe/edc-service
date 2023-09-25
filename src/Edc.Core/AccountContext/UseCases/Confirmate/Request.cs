using System.Text.Json.Serialization;
using Edc.Core.SharedContext.UseCases;
using MediatR;

namespace Edc.Core.AccountContext.UseCases.Confirmate;

public class Request : IRequest<Response<ResponseData>> {
    public Request(Guid id, string code)
    {
        Id = id;
        Code = code;
    }
    
    [JsonConstructor]
    public Request(string code)
    {
        Code = code;
    }
    public Guid Id { get; private set; }
    public string Code { get; }

    public void SetId(Guid id) {
        Id = id;
    }
}
