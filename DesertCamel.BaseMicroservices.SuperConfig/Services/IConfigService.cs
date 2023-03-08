using DesertCamel.BaseMicroservices.SuperConfig.Models;

namespace DesertCamel.BaseMicroservices.SuperConfig.Services
{
    public interface IConfigService
    {
        Task<FuncResponse<ConfigCreateResponseModel>> Create(ConfigCreateRequestModel createRequest);
        Task<FuncResponse<ConfigGetResponseModel>> Get(ConfigGetRequestModel getRequest);
        Task<FuncResponse<ConfigDeleteResponseModel>> Delete(ConfigDeleteRequestModel deleteRequest);
    }
}
