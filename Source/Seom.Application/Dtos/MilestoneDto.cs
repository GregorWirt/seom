using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;

namespace Seom.Application.Dtos
{
    public record MilestoneDto(
        Guid Guid,
        string Name,
        DateTime DatePlanned,
        DateTime? DateFinished);
}
