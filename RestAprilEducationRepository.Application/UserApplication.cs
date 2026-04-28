using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using RestAprilEducationRepository.Domain;

namespace RestAprilEducationRepository.Application
{
    internal class UserApplication(UserManager<AppUser> userManager)
    {
    }
}
