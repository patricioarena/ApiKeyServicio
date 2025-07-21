using System.Collections.Generic;
using System.Linq;
using Application.IFactory;
using DataAccess.Models;
using Domain.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class ServiceApplication : ServiceGeneral, IServiceApplication
    {
        private readonly DbContext _Context;
        private readonly IAbstractServiceFactory _Service;

        public ServiceApplication(ApiKeyDbContext context, IAbstractServiceFactory service)
        {
            _Context = context;
            _Service = service;
        }

        public DataAccess.Models.Application GetAppById(int appId)
        {
            return _Context.Set<DataAccess.Models.Application>().FirstOrDefault(e => e.id.Equals(appId));
        }

        public List<DataAccess.Models.Application> GetApps()
        {
            return _Context.Set<DataAccess.Models.Application>().ToList();
        }

        public int Save(ApplicationDTO appDTO)
        {
            appDTO.name = NormalizeString(appDTO.name);
            DataAccess.Models.Application app = _Service.Mapper().Map<DataAccess.Models.Application>(appDTO);
 
            _Context.Set<DataAccess.Models.Application>().Add(app);
            _Context.SaveChanges();
            int id = app.id;
            return id;
        }
    }
}
