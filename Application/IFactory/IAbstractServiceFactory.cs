using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Application.IFactory
{
    public interface IAbstractServiceFactory
    {
        public IMapper Mapper();
        public ILogger Logger();
    }
}
