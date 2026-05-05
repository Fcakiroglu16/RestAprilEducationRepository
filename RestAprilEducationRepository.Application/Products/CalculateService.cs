using System;
using System.Collections.Generic;
using System.Text;
using RestAprilEducationRepository.Application.Products.GetList;

namespace RestAprilEducationRepository.Application.Products
{
    public class CalculateService() : ICalculateService
    {
        public decimal CalculatePriceWithTax(decimal price, decimal taxRate)
        {
            return price + (price * taxRate);
        }
    }
}
