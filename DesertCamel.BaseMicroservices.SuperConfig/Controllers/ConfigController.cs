using DesertCamel.BaseMicroservices.SuperConfig.Models;
using DesertCamel.BaseMicroservices.SuperConfig.Services;
using Microsoft.AspNetCore.Mvc;

namespace DesertCamel.BaseMicroservices.SuperConfig.Controllers
{
    [ApiController]
    [Route("config")]
    public class ConfigController : ControllerBase
    {
        private readonly IConfigService _configService;
        private readonly ILogger<ConfigController> _logger;

        public ConfigController(
            IConfigService configService,
            ILogger<ConfigController> logger)
        {
            _configService = configService;
            _logger = logger;
        }

        [HttpPost("create")]
        public async Task<FuncResponse<ConfigCreateResponseModel>> Create([FromBody] ConfigCreateRequestModel createRequest)
        {
            var createResult = await _configService.Create(createRequest);
            if (createResult.IsError())
            {
                _logger.LogError(createResult.ErrorMessage);
                return new FuncResponse<ConfigCreateResponseModel>
                {
                    ErrorMessage = "create config error"
                };
            }
            return createResult;
        }

        [HttpPost("get")]
        public async Task<FuncResponse<ConfigGetResponseModel>> Get([FromBody] ConfigGetRequestModel getRequest)
        {
            var getResult = await _configService.Get(getRequest);
            if (getResult.IsError())
            {
                _logger.LogInformation(getResult.ErrorMessage);
                return new FuncResponse<ConfigGetResponseModel>
                {
                    ErrorMessage = "get config failed"
                };
            }
            return getResult;
        }

        [HttpPost("delete")]
        public async Task<FuncResponse<ConfigDeleteResponseModel>> Delete([FromBody] ConfigDeleteRequestModel deleteRequest)
        {
            var deleteResult = await _configService.Delete(deleteRequest);
            if (deleteResult.IsError())
            {
                _logger.LogError(deleteResult.ErrorMessage);
                return new FuncResponse<ConfigDeleteResponseModel>
                {
                    ErrorMessage = "delete config failed"
                };
            }
            return deleteResult;
        }
    }
}
