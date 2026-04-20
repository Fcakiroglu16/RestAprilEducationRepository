using System;
using System.Collections.Generic;
using System.Text;

namespace RestAprilEducationRepository.Application
{
    internal interface IImageProcess
    {
        void Process(string path);
    }

    public class ImageProcess : IImageProcess
    {
        public void Process(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                // 

                throw new Exception("path değeri boş olamaz");
            }

            // Görüntü işleme kodları burada olacak
        }
    }
}
