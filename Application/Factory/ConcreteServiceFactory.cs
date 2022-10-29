using Application.IFactory;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Factory
{
    public class ConcreteServiceFactory : IAbstractServiceFactory
    {
        public IMapper Mapper { get; }
        public ILogger Logger { get; }
        public ConcreteServiceFactory(IMapper mapper, ILogger logger)
        {
            Logger = logger;
            Mapper = mapper;
        }

        IMapper IAbstractServiceFactory.Mapper()
        {
            return Mapper;
        }

        ILogger IAbstractServiceFactory.Logger()
        {
            return Logger;
        }
    }
}
