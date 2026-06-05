namespace Domain.Abstractions;

public class ServiceResult
{
    public ServiceResult()
    {
        IsSuccess = true;
        Message = string.Empty;
    }

    public ServiceResult(string message)
    {
        IsSuccess = false;
        Message = message;
    }

    public bool IsSuccess { get; set; }
    public string Message { get; set; }
}

public class ServiceResult<T> : ServiceResult
{
    public ServiceResult(string message)
    {
        IsSuccess = false;
        Message = message;
        Data = default;
    }

    public ServiceResult(string message, T data)
    {
        IsSuccess = false;
        Message = message;
        Data = data;
    }

    public ServiceResult(T model)
    {
        Message = string.Empty;
        IsSuccess = true;
        Data = model;
    }

    public T Data { get; set; }
}