using System;
using System.Collections.Generic;
using System.Text;

namespace RestAprilEducationRepository.Domain
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string Barcode { get; set; } = null!;
    }
}
