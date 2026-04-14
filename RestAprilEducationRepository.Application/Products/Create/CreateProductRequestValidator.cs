using FluentValidation;

namespace RestAprilEducationRepository.Application.Products.Create;

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator(IProductRepository productRepository)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("isim alanı boş olamaz")
            .Must(productName => productName.StartsWith("A"))
            .WithMessage("ürün ismi büyük A harfi ile başlamalıdır");
        RuleFor(x => x.Price)
            .InclusiveBetween(1, 1000).WithMessage("fiyat değeri 1 ile 1000 arasında olmalıdır");
    }
}