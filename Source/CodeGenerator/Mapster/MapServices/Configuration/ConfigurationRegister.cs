using CleanModels.Benchmark;
using Mapster;

namespace CodeGenerator.Mapster.MapServices.Configuration;

public class ConfigurationRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // config
        //     .NewConfig<DbLoader.PersistModels.User, DomainModels.User>()
        //     .Map(d => d.Role, s => (RoleType)s.Role)
        //     .Map(d => d.Name, s => $"{s.FName} {s.LName}")
        //     .TwoWays()
        //     .GenerateMapper(MapType.MapToTarget | MapType.Map);
        //
        // config
        //     .NewConfig<DbLoader.PersistModels.Transaction, DomainModels.Transaction>()
        //     .Map(d => d.Currency, s => (CurrencyType)s.Currency)
        //     .Map(d => d.Date, s => s.TransactDate)
        //     .Map(d => d.TransactionSum, s => s.Amount)
        //     .TwoWays()
        //     .GenerateMapper(MapType.MapToTarget | MapType.Map);

        config
            .NewConfig<Benchmark, Dal.Entities.Benchmark>()
            .TwoWays()
            .GenerateMapper(MapType.MapToTarget | MapType.Map);
    }
}