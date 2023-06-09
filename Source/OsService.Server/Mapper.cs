using CleanModels;
using ProtoConfigurationVerification = OsInfoService.ConfigurationVerification;

namespace OsService.Server;

public static class Mapper
{
    public static ProtoConfigurationVerification? MapToProtoConfigurationVerification(this ConfigurationVerification? model)
    {
        if (model == null)
        {
            return null;
        }

        var resultModel = new ProtoConfigurationVerification
        {
            VerificationCommand = model.VerificationCommand,
            VerificationResult = model.VerificationResult,
        };

        return resultModel;
    }
}