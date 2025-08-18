using System;

namespace ApiKeyPOC.Configs
{
    public interface IAuthenticaConfig
    {
        bool ValidRangeOn();
        int GetApplicationId();
    }
}