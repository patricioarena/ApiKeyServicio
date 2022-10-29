using DataAccess.Models;
using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IServiceLogApikeyDB
    {
        List<LogDTO> GetLogs();
        List<LogDTO> ByClient(int id);
        List<LogDTO> ByClientKey(Guid key);
        int LogDb(RequestDTO request, string descriptionError);
    }
}
