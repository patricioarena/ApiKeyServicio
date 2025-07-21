using Application.IFactory;
using Application.IServices;
using DataAccess.Models;
using Domain.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ServiceLog : IServiceLogApikeyDB
    {
        private readonly DbContext _Context;
        private readonly IAbstractServiceFactory _Service;

        public ServiceLog(ApiKeyDbContext context, IAbstractServiceFactory service)
        {
            _Context = context;
            _Service = service;
        }

        public List<LogDto> GetLogs()
        {
            var list = _Context.Set<Log>()
                .Include(e => e.client)
                .Include(e => e.application)
                .ToList();

            List<LogDto> listLogDto = _Service.Mapper().Map<List<Log>, List<LogDto>>(list);

            return listLogDto;
        }

        public List<LogDto> ByClient(int id)
        {
            var list = _Context.Set<Log>().Where(e => e.clientId.Equals(id))
                .Include(e => e.client)
                .Include(e => e.application)
                .ToList();

            List<LogDto> listLogDto = _Service.Mapper().Map<List<Log>, List<LogDto>>(list);

            return listLogDto;
        }

        public List<LogDto> ByClientKey(Guid key)
        {
            var list = _Context.Set<Log>().Where(e => e.apiKey.Equals(key))
                .Include(e => e.client)
                .Include(e => e.application)
                .ToList();

            List<LogDto> listLogDto = _Service.Mapper().Map<List<Log>, List<LogDto>>(list);

            return listLogDto;
        }



        public int LogDb(RequestDto request, string descriptionError)
        {
            Log o = new Log()
            {
                applicationId = request.appId,
                clientId = request.clientId,
                apiKey = request.apiKey,
                remoteIp = request.remoteIp,
                description = descriptionError
            };

            Log log = _Service.Mapper().Map<Log>(o);

            _Context.Set<Log>().Add(log);
            _Context.SaveChanges();

            return log.id;
        }


    }
}
