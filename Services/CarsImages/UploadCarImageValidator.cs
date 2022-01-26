using FluentValidation;

namespace Services.CarsImages;

public class UploadCarImageValidator : AbstractValidator<UploadCarImage.Command>
{
    public UploadCarImageValidator()
    {
        RuleFor(x => x.Plate).NotEmpty();
        RuleFor(x => x.File.Length).NotNull().LessThanOrEqualTo(1 * 1024 * 1024) // 1mb
            .WithMessage("File size is larger than allowed limit 1MB");
        RuleFor(x => x.File.ContentType).Must(x => x.Contains("image")).WithMessage("File must be an image").NotEmpty();
    }
}
