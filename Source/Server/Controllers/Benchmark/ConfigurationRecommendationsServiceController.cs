using CommonService;
using ConfigurationRecommendationsService;
using Dal.Entities;
using Grpc.Core;
using Server.Services.Base;
using static ConfigurationRecommendationsService.ConfigurationRecommendations;

namespace Server.Controllers;

/// <inheritdoc />
public class ConfigurationRecommendationsServiceController : ConfigurationRecommendationsBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationRecommendationsServiceController"/> class.
    /// </summary>
    /// <param name="service">Service.</param>
    public ConfigurationRecommendationsServiceController(IDbMultipleService<ConfigurationRecommendation> service)
    {
        Service = service;
    }

    private IDbMultipleService<ConfigurationRecommendation> Service { get; }

    /// <inheritdoc/>
    public override async Task<GetResponse> Get(GetRequest model, ServerCallContext context)
    {
        var response = await GetAsync(model);

        return response;
    }

    /// <inheritdoc/>
    public override async Task<GetResponse> GetExist(Request model, ServerCallContext context)
    {
        var response = await GetExistAsync(model);

        return response;
    }

    /// <inheritdoc/>
    public override async Task<CommandResponse> Add(Request model, ServerCallContext context)
    {
        var response = await AddAsync(model);

        return response;
    }

    // /// <inheritdoc/>
    // public override async Task<CommandResponse> Update(Request model, ServerCallContext context)
    // {
    //     var response = await UpdateAsync(model);
    //
    //     return response;
    // }

    /// <inheritdoc/>
    public override async Task<CommandResponse> Remove(Request model, ServerCallContext context)
    {
        var response = await RemoveAsync(model);

        return response;
    }

    private async Task<GetResponse> GetAsync(GetRequest model)
    {
        var queryResult = await Service.GetPageAsync(model.PageModel.MapToPageModel());
        if (!queryResult.IsSuccessful)
        {
            return new GetResponse { ErrorMessage = queryResult.ErrorMessage };
        }

        var data = new ConfigurationRecommendationsData();
        var protoModels = queryResult.Data.MapToProtoConfigurationRecommendations();
        data.Recommendations.AddRange(protoModels);

        return new GetResponse { Data = data };
    }

    private async Task<GetResponse> GetExistAsync(Request model)
    {
        var queryResult = await Service.GetExistAsync(model.Recommendations.MapToRecommendations().ToList());
        if (!queryResult.IsSuccessful)
        {
            return new GetResponse { ErrorMessage = queryResult.ErrorMessage };
        }

        var data = new ConfigurationRecommendationsData();
        data.Recommendations.AddRange(queryResult.Data.MapToProtoConfigurationRecommendations());

        return new GetResponse { Data = data };
    }

    private async Task<CommandResponse> AddAsync(Request model)
    {
        var recommendations = model.Recommendations.MapToRecommendations().ToList();
        var queryResult = await Service.AddAsync(recommendations);
        if (!queryResult.IsSuccessful)
        {
            return new CommandResponse { ErrorMessage = queryResult.ErrorMessage };
        }

        return new CommandResponse();
    }

    private async Task<CommandResponse> UpdateAsync(Request model)
    {
        var recommendations = model.Recommendations.MapToRecommendations().ToList();
        var queryResult = await Service.UpdateAsync(recommendations);
        if (!queryResult.IsSuccessful)
        {
            return new CommandResponse { ErrorMessage = queryResult.ErrorMessage };
        }

        return new CommandResponse();
    }

    private async Task<CommandResponse> RemoveAsync(Request model)
    {
        var recommendations = model.Recommendations.MapToRecommendations().ToList();
        var queryResult = await Service.RemoveAsync(recommendations);
        if (!queryResult.IsSuccessful)
        {
            return new CommandResponse { ErrorMessage = queryResult.ErrorMessage };
        }

        return new CommandResponse();
    }
}