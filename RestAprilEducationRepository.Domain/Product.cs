using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using RestAprilEducationRepository.Domain.Exceptions;

namespace RestAprilEducationRepository.Domain
{
    // object = data + behavior => rich domain model / anemic domain model
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;


        public decimal Price { get; set; }


        public string Barcode { get; set; } = null!;


        public int CategoryId { get; set; }

        public Category Category { get; set; } = null!;


        public void SetPrice(decimal price)
        {
            if (price < 0)
            {
                throw new Exception("fiyat alanı 0'dan küçük olamaz");
            }

            Price = price;
        }
    }
}
