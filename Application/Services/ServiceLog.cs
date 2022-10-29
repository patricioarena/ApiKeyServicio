using Application.IFactory;
using Application.IServices;
using DataAccess.Models;
using Domain.DTOs;
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

        public List<LogDTO> GetLogs()
        {
            var list = _Context.Set<Log>()
                .Include(e => e.client)
                .Include(e => e.application)
                .ToList();

            List<LogDTO> listLogDTO = _Service.Mapper().Map<List<Log>, List<LogDTO>>(list);

            return listLogDTO;
        }

        public List<LogDTO> ByClient(int id)
        {
            var list = _Context.Set<Log>().Where(e => e.clientId.Equals(id))
                .Include(e => e.client)
                .Include(e => e.application)
                .ToList();

            List<LogDTO> listLogDTO = _Service.Mapper().Map<List<Log>, List<LogDTO>>(list);

            return listLogDTO;
        }

        public List<LogDTO> ByClientKey(Guid key)
        {
            var list = _Context.Set<Log>().Where(e => e.apiKey.Equals(key))
                .Include(e => e.client)
                .Include(e => e.application)
                .ToList();

            List<LogDTO> listLogDTO = _Service.Mapper().Map<List<Log>, List<LogDTO>>(list);

            return listLogDTO;
        }



        public int LogDb(RequestDTO request, string descriptionError)
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
