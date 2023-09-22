namespace Edc.Core.SharedContext.UseCases;

public class Response<T> {
    public Response(T data) {
        Data = data;
        IsSuccess = true;
    }

    public Response() {
        
    }
    
    public bool IsSuccess { get; set; } = false;
    public T? Data { get; set; } = default;
}