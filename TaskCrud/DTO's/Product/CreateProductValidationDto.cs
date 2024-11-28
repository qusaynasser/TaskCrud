using FluentValidation;

namespace TaskCrud.DTO_s.Product
{
    public class CreateProductValidationDto: AbstractValidator<ProductDto>
    {
        public CreateProductValidationDto() 
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Requerd")
                .Length(5, 30).WithMessage("at least 5 and max 30");

            RuleFor(p => p.Price).NotEmpty().WithMessage("Requerd")
                .InclusiveBetween(20,3000).WithMessage("price from 20 to 3000");

            RuleFor(p => p.Description).NotEmpty().WithMessage("Requerd")
                .MaximumLength(10).WithMessage("Min 10");
        }
    }
}
