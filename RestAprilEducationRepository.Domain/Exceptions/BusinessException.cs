using System;
using System.Collections.Generic;
using System.Text;

namespace RestAprilEducationRepository.Domain.Exceptions
{
    public class BusinessException(string message) : Exception(message)
    {
        public string? ErrorDetail { get; set; }
    }
}
