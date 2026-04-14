using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RestAprilEducationRepository.Application.Products.Create
{
    public record CreateProductRequest(
        //[Required(ErrorMessage = "isim alanı boş olamaz")]
        string Name,

        //[Range(1, 1000, ErrorMessage = "fiyat değeri 1 ile 1000 arasında olmalıdır")]
        decimal Price);
}
