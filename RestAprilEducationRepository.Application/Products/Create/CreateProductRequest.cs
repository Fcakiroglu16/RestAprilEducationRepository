using System;
using System.Collections.Generic;
using System.Text;

namespace RestAprilEducationRepository.Application.Products.Create
{
    public record CreateProductRequest(string Name, decimal Price);
}
