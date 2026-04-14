using System;
using System.Collections.Generic;
using System.Text;

namespace RestAprilEducationRepository.Domain
{
    // object = data + behavior
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;


        public decimal Price
        {
            get => field;
            set
            {
                if (value is < 1 or > 1000)
                {
                    throw new Exception("fiyat alanı 1 ile 1000 arasında olmalıdır");
                }

                field = value;
            }
        }

        //public void SetPrice(decimal price)
        //{
        //    if (price is < 1 or > 1000)
        //    {
        //        throw new Exception("fiyat alanı 1 ile 1000 arasında olmalıdır");
        //    }
        //    Price=price;
        //}

        public string Barcode { get; set; } = null!;
    }
}
