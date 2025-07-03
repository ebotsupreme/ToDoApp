namespace ToDoApp.Shared;
public class Result<T>(bool success, T? data, string? error)
{
    public bool Success { get; } = success;
    public string? ErrorMessage { get; } = error;
    public T? Data { get; } = data;

    public static Result<T> Ok(T data) => new(true, data, null);
    public static Result<T> Fail(string error) => new(false, default, error);
}
