using FluentValidation.Results;

namespace Application.Core;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public bool IsSuccessNoContent { get; set; }
    public T Value { get; set; }
    public string Error { get; set; }
    public ValidationResult FluentValidationError { get; set; } = null;

    public static Result<T> Success(T value)
    {
        return new Result<T>
        {
            IsSuccess = true,
            Value = value
        };
    }

    public static Result<T> SuccessNoContent(T value)
    {
        return new Result<T>
        {
            IsSuccess = true,
            IsSuccessNoContent = true,
            Value = value
        };
    }

    public static Result<T> Failure(string error, ValidationResult fluentValidationError = null)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Error = error,
            FluentValidationError = fluentValidationError
        };
    }
}
