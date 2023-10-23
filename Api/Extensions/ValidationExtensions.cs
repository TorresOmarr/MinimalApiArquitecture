using FluentValidation.Results;

namespace Api.Extensions;


public static class ValidationExtensions
{

    public static Dictionary<string, string[]> GetValidationProblems(this ValidationResult result)
    {
        return (from e in result.Errors
                group e.ErrorMessage by e.PropertyName).ToDictionary((IGrouping<string, string> failureGroup) => failureGroup.Key, (IGrouping<string, string> failureGroup) => failureGroup.ToArray());
    }
}