namespace RestAprilEducationRepository.Application.Products
{
    public interface ICalculateService
    {
        decimal CalculatePriceWithTax(decimal price, decimal taxRate);
    }
}