using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IServiceApplication
    {
        DataAccess.Models.Application GetAppById(int appId);
        List<DataAccess.Models.Application> GetApps();
        int Register(ApplicationDTO app);
    }
}
