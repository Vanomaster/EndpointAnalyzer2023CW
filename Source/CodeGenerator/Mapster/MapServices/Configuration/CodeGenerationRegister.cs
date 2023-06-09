using CleanModels.Benchmark;
using Mapster;

namespace CodeGenerator.Mapster.MapServices.Configuration;

public class CodeGenerationRegister : ICodeGenerationRegister
{
    // private readonly Type[,] types =
    // {
    //     { typeof(Benchmark), typeof(MainBenchmark) },
    // };

    private readonly TypesRelation[] types =
    {
        new ()
        {
            FirstType = typeof(Benchmark),
            SecondType = typeof(Dal.Entities.Benchmark),
        },

    };

    public void Register(CodeGenerationConfig config)
    {
        foreach (var type in types)
        {
            var attribute = new AdaptFromAttribute(type.FirstType);
            var attributeBuilder = new AdaptAttributeBuilder(attribute);
            attributeBuilder.ForTypes(type.SecondType);
            config.AdaptAttributeBuilders.Add(attributeBuilder);
        }

        // var userAttribute = new AdaptFromAttribute(typeof(DomainModels.User));
        // var userAttrBuilder = new AdaptAttributeBuilder(userAttribute);
        // userAttrBuilder.ForTypes();
        //
        // var transactionAttribute = new AdaptFromAttribute(typeof(DomainModels.Transaction));
        // var transactionAttrBuilder = new AdaptAttributeBuilder(transactionAttribute);
        // transactionAttrBuilder.ForType<DbLoggerCategory.Database.Transaction>();
        //
        // var userTransactionAttribute = new AdaptFromAttribute(typeof(DomainModels.UserTransaction));
        // var userTransactionAttrBuilder = new AdaptAttributeBuilder(userTransactionAttribute);
        // userTransactionAttrBuilder.ForType<UserTransaction>();
        //
        // config.AdaptAttributeBuilders.Add(userAttrBuilder);
        // config.AdaptAttributeBuilders.Add(transactionAttrBuilder);
        // config.AdaptAttributeBuilders.Add(userTransactionAttrBuilder);
    }

    private class TypesRelation
    {
        public Type FirstType { get; init; }

        public Type SecondType { get; init; }
    }
}