using System;
using System.Collections.Generic;
using System.Text;

namespace RestAprilEducationRepository.Domain
{
    public class UserDetail
    {
        public Guid UserId { get; set; }

        public string Address { get; set; }

        public AppUser AppUser { get; set; }
    }
}