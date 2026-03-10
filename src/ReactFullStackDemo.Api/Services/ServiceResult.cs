namespace ReactFullStackDemo.Api.Services;

public sealed class ServiceResult<T>
{
    private ServiceResult(T value)
    {
        IsSuccess = true;
        Value = value;
        Errors = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);
    }

    private ServiceResult(IReadOnlyDictionary<string, string[]> errors, bool notFound)
    {
        IsSuccess = false;
        IsNotFound = notFound;
        Errors = errors;
    }

    public bool IsSuccess { get; }
    public bool IsNotFound { get; }
    public T? Value { get; }
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public static ServiceResult<T> Success(T value) => new(value);

    public static ServiceResult<T> Validation(IReadOnlyDictionary<string, string[]> errors)
        => new(errors, notFound: false);

    public static ServiceResult<T> NotFound()
        => new(new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase), notFound: true);
}
