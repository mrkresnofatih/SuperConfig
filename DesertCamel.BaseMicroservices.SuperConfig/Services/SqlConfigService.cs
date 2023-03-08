using DesertCamel.BaseMicroservices.SuperConfig.Entities;
using DesertCamel.BaseMicroservices.SuperConfig.EntityFramework;
using DesertCamel.BaseMicroservices.SuperConfig.Models;
using DesertCamel.BaseMicroservices.SuperConfig.Utilities;
using Microsoft.EntityFrameworkCore;

namespace DesertCamel.BaseMicroservices.SuperConfig.Services
{
    public class SqlConfigService : IConfigService
    {
        private readonly SuperConfigDbContext _superConfigDbContext;
        private readonly ILogger<SqlConfigService> _logger;

        public SqlConfigService(
            SuperConfigDbContext superConfigDbContext,
            ILogger<SqlConfigService> logger)
        {
            _superConfigDbContext = superConfigDbContext;
            _logger = logger;
        }

        public async Task<FuncResponse<ConfigCreateResponseModel>> Create(ConfigCreateRequestModel createRequest)
        {
            try
            {
                _logger.LogInformation($"Start CreateConfig w. data: {createRequest.ToJson()}");
                var newConfig = new ConfigEntity
                {
                    Id = Guid.NewGuid(),
                    Key = createRequest.Key,
                    Description = createRequest.Description,
                    Value = createRequest.Value
                };
                await _superConfigDbContext.Configs.AddAsync(newConfig);
                await _superConfigDbContext.SaveChangesAsync();

                _logger.LogInformation("CreateConfig success");
                return new FuncResponse<ConfigCreateResponseModel>
                {
                    Data = new ConfigCreateResponseModel
                    {
                        Key = createRequest.Key,
                        Value = createRequest.Value,
                        Description = createRequest.Description
                    }
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "failed to create config");
                return new FuncResponse<ConfigCreateResponseModel>
                {
                    ErrorMessage = "CreateConfig failed"
                };
            }
        }

        public async Task<FuncResponse<ConfigDeleteResponseModel>> Delete(ConfigDeleteRequestModel deleteRequest)
        {
            try
            {
                _logger.LogInformation($"Start DeleteConfig w. data: {deleteRequest.ToJson()}");
                var configFound = await _superConfigDbContext
                    .Configs
                    .Where(x => x.Key.Equals(deleteRequest.Key))
                    .FirstOrDefaultAsync();
                if (configFound == null)
                {
                    throw new Exception("config for delete operation not found");
                }

                _superConfigDbContext.Configs.Remove(configFound);
                await _superConfigDbContext.SaveChangesAsync();

                _logger.LogInformation("success delete config");
                return new FuncResponse<ConfigDeleteResponseModel>
                {
                    Data = new ConfigDeleteResponseModel
                    {
                        Key = deleteRequest.Key
                    }
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "failed to delete config");
                return new FuncResponse<ConfigDeleteResponseModel>
                {
                    ErrorMessage = "failed to delete config"
                };
            }
        }

        public async Task<FuncResponse<ConfigGetResponseModel>> Get(ConfigGetRequestModel getRequest)
        {
            try
            {
                _logger.LogInformation($"Start GetConfig w. data: {getRequest.ToJson()}");
                var configFound = await _superConfigDbContext
                    .Configs
                    .Where(x => x.Key.Equals(getRequest.Key))
                    .FirstOrDefaultAsync();
                if (configFound == null)
                {
                    throw new Exception("config for delete operation not found");
                }

                _logger.LogInformation("getconfig success");
                return new FuncResponse<ConfigGetResponseModel>
                {
                    Data = new ConfigGetResponseModel
                    {
                        Key = configFound.Key,
                        Value = configFound.Value,
                        Description = configFound.Description
                    }
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "getconfig error");
                return new FuncResponse<ConfigGetResponseModel>
                {
                    ErrorMessage = "get config failed"
                };
            }
        }
    }
}
