using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seom.Application.Dtos
{
    public record WorkItemDto(
        Guid Guid,
        [StringLength(255, MinimumLength = 2)] string Name,
        DateTime From,
        DateTime To) : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (From < DateTime.UtcNow.AddYears(-1))
                yield return new ValidationResult("Start date is over 1 year in the past.", new string[] { nameof(From) });
            if (To > DateTime.UtcNow.AddYears(1))
                yield return new ValidationResult("End date is over 1 year in the future.", new string[] { nameof(From) });
            if (From > To)
                yield return new ValidationResult("Start date is after end date.", new string[] { nameof(From), nameof(To) });
        }
    }
}
