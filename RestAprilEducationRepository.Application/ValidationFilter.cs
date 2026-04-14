#region

using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

#endregion

namespace RestAprilEducationRepository.Application;

public class ValidationFilter<T> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        IValidator<T>? validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();

        if (validator is null) return await next(context);

        T? requestModel = context.Arguments.OfType<T>().SingleOrDefault();


        if (requestModel is null) return await next(context);

        ValidationResult validateResult = await validator.ValidateAsync(requestModel);

        if (!validateResult.IsValid) return Results.ValidationProblem(validateResult.ToDictionary());


        return await next(context);
    }
}