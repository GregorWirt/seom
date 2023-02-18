using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Seom.Application.Dtos
{
    public record ProjectDto(
        Guid Guid,
        [StringLength(255, MinimumLength = 2)] string Name,
        [StringLength(255, MinimumLength = 2)] string Customer,
        [StringLength(255, MinimumLength = 2)] string? Repo, DateTime Start, DateTime? Finished) : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Start < DateTime.UtcNow.AddYears(-1))
                yield return new ValidationResult("Start date is over 1 year in the past.", new string[] { nameof(Start) });
            if (Finished.HasValue && Start > Finished.Value)
                yield return new ValidationResult("Start date is after end date.", new string[] { nameof(Start), nameof(Finished) });
        }
    }
}
