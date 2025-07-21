using DataAccess.Models;
using Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IServiceLogApikeyDB
    {
        List<LogDto> GetLogs();
        List<LogDto> ByClient(int id);
        List<LogDto> ByClientKey(Guid key);
        int LogDb(RequestDto request, string descriptionError);
    }
}
