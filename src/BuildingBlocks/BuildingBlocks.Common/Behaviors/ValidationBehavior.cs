using BuildingBlocks.Common.Results;
using FluentValidation;
using MediatR;

namespace MyStoreProject.Services.Ordering.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (validators.Any())
        {
            var validationResults = await Task.WhenAll(
                validators.Select(v =>
                    v.ValidateAsync(new ValidationContext<TRequest>(request), cancellationToken)));

            var failures = validationResults
                .SelectMany(validationResult => validationResult.Errors)
                .Where(validationFailure => validationFailure is not null)
                .Select(failure => Error.Validation(failure.PropertyName, failure.ErrorMessage))
                .Distinct()
                .ToArray();

            if (failures.Length != 0)
            {
                return CreateValidationResult<TResponse>(failures[0]);
            }
        }

        return await next();
    }
    
    private static TResponse CreateValidationResult<T>(Error error)
        where T : Result
    {
        object validationResult = typeof(Result<T>)
            .MakeGenericType(typeof(T).GenericTypeArguments[0])
            .GetMethod(nameof(Result.Failure))!
            .Invoke(null, [error])!;

        return (TResponse)validationResult;
    }
    
}