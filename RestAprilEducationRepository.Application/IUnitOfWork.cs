using System;
using System.Collections.Generic;
using System.Text;

namespace RestAprilEducationRepository.Application
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync();
    }
}
