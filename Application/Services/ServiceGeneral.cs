using Application.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ServiceGeneral: IServiceGeneral
    {
        public string NormalizeString(string param)
        {
            var normalizeString = param.ToUpper();
            normalizeString = normalizeString.Replace(" ", "_");
            return normalizeString;
        }
    }
}
