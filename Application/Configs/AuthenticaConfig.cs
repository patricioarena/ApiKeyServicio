using System;
using Microsoft.Extensions.Configuration;

namespace ApiKeyPOC.Configs
{
    public class AuthenticaConfig : IAuthenticaConfig
    {
        private bool _validRangeOn;
        private int _applicationId;
                 
        public AuthenticaConfig(IConfiguration configuration)
        {
            bool.TryParse(configuration.GetSection("Authentica:ValidRangeOn").Value, out _validRangeOn);
            _applicationId = Int32.Parse(configuration.GetSection("Authentica:ApplicationId").Value);
        }

        public bool ValidRangeOn() => _validRangeOn;

        public int GetApplicationId() => _applicationId;
    }
}