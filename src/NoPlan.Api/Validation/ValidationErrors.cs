using System.Diagnostics;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace NoPlan.Api.Validation;

public static class ValidationErrors
{
#pragma warning disable CA1002
    public static object ResponseBuilder(List<ValidationFailure> failures, HttpContext context, int status)
#pragma warning restore CA1002
    {
        ArgumentNullException.ThrowIfNull(context);

        var problemDetails = new ValidationProblemDetails
        {
            Type = "https://www.rfc-editor.org/rfc/rfc7231#section-6.5.1",
            Status = status,
            Extensions = { ["traceId"] = Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier }
        };

        foreach (var failure in failures.GroupBy(f => f.PropertyName))
        {
            problemDetails.Errors[failure.Key] = failure.Select(g => g.ErrorMessage).ToArray();
        }

        return problemDetails;
    }
}
