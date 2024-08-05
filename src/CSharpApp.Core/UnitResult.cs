namespace CSharpApp.Core;

// DISCLAIMER: This is partially copied and thinned down, quickly and dirtily from https://github.com/vkhorikov/CSharpFunctionalExtensions
// The thought process behind it is that, I didn't want to introduce a library which maybe is not known by the evaluator of the task
// In a real world scenario we would have a talk about using it ;)

public readonly struct UnitResult<TError> 
{
    public bool IsFailure { get; }
    public bool IsSuccess => !IsFailure;
    
    private readonly TError? _error;
    public TError? Error => IsFailure ? _error : throw new Exception("UnitResult is Success, so no Error exists");

    private UnitResult(bool isFailure, TError? error)
    {
        IsFailure = isFailure;
        _error = error;
    }
    
    public static UnitResult<TError> Success() => new(false, default!);
    public static UnitResult<TError> Failure(TError error) => new(true, error);
    
    public static implicit operator UnitResult<TError>(TError error)
    {
        if (error is UnitResult<TError> result)
        {
            return result;
        }

        return Failure(error);
    }
}