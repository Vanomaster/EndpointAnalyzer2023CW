using CleanModels;
using ProtoConfigurationVerification = OsInfoService.ConfigurationVerification;

namespace Server.Client;

public static class Mapper
{
    public static ConfigurationVerification? MapToConfigurationVerification(this ProtoConfigurationVerification? model)
    {
        if (model == null)
        {
            return null;
        }

        var resultModel = new ConfigurationVerification
        {
            VerificationCommand = model.VerificationCommand,
            VerificationResult = model.VerificationResult,
        };

        return resultModel;
    }
}