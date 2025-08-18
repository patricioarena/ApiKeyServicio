using AutoMapper;
using DataAccess.Models;
using Domain.Data;

namespace Application
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration(string profileName) : base(profileName)
        {
            CreateMap<Client, ClientDto>()
            .ForMember(destino => destino.clientName, options => options.MapFrom(origen => origen.name))
            .ReverseMap();

            CreateMap<Key, AssingnKeyDto>()
            .ForMember(destino => destino.clientId, options => options.MapFrom(origen => origen.clientId))
            .ReverseMap();

            CreateMap<Referer, AssingnKeyDto>()
            .ForMember(destino => destino.ipStart, options => options.MapFrom(origen => origen.ipStart))
            .ForMember(destino => destino.ipEnd, options => options.MapFrom(origen => origen.ipEnd))
            .ForMember(destino => destino.referer, options => options.MapFrom(origen => origen.name))
            .ReverseMap();
            
            CreateMap<DataAccess.Models.Application, ApplicationDto>()
            .ForMember(destino => destino.name, options => options.MapFrom(origen => origen.name))
            .ForMember(destino => destino.description, options => options.MapFrom(origen => origen.description))
            .ReverseMap();

            CreateMap<Log, LogDto>()
            .ForMember(destino => destino.id, options => options.MapFrom(origen => origen.id))
            .ForMember(destino => destino.clientName, options => options.MapFrom(origen => origen.client.name))
            .ForMember(destino => destino.clientId, options => options.MapFrom(origen => origen.clientId))
            .ForMember(destino => destino.apiKey, options => options.MapFrom(origen => origen.apiKey))
            .ForMember(destino => destino.applicationName, options => options.MapFrom(origen => origen.application.name))
            .ForMember(destino => destino.applicationId, options => options.MapFrom(origen => origen.applicationId))
            .ForMember(destino => destino.remoteIp, options => options.MapFrom(origen => origen.remoteIp))
            .ForMember(destino => destino.incidentDate, options => options.MapFrom(origen => origen.incidentDate))
            .ForMember(destino => destino.description, options => options.MapFrom(origen => origen.description))
            .ReverseMap();
        }
    }
}