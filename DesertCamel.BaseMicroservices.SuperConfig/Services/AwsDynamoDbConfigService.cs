using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using DesertCamel.BaseMicroservices.SuperConfig.Models;
using DesertCamel.BaseMicroservices.SuperConfig.Models.Configs;
using DesertCamel.BaseMicroservices.SuperConfig.Utilities;
using Microsoft.Extensions.Options;

namespace DesertCamel.BaseMicroservices.SuperConfig.Services
{
    public class AwsDynamoDbConfigService : IConfigService
    {
        private readonly ILogger<AwsDynamoDbConfigService> _logger;
        private readonly AwsDynamoDbConfig _awsDynamoDbConfig;
        private readonly AmazonDynamoDBClient _amazonDynamoDBClient;

        public AwsDynamoDbConfigService(
            ILogger<AwsDynamoDbConfigService> logger,
            IOptions<AwsDynamoDbConfig> awsDynamoDbConfig)
        {
            _logger = logger;
            _awsDynamoDbConfig = awsDynamoDbConfig.Value;
            _amazonDynamoDBClient = new AmazonDynamoDBClient(new AmazonDynamoDBConfig
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(_awsDynamoDbConfig.Region),
            });
        }

        public async Task<FuncResponse<ConfigCreateResponseModel>> Create(ConfigCreateRequestModel createRequest)
        {
            try
            {
                _logger.LogInformation($"Start CreateConfig w. data: {createRequest.ToJson()}");
                if (String.IsNullOrWhiteSpace(_awsDynamoDbConfig.TableName))
                {
                    throw new Exception("tablename in aws dynamodb config is not set");
                }

                var table = Table.LoadTable(_amazonDynamoDBClient, _awsDynamoDbConfig.TableName);
                var newConfig = new Document();
                
                if (String.IsNullOrWhiteSpace(_awsDynamoDbConfig.PartitionKeyName))
                {
                    throw new Exception("partition key in aws dynamodb config is not set");
                }

                newConfig[_awsDynamoDbConfig.PartitionKeyName] = createRequest.Key;
                if (!String.IsNullOrWhiteSpace(_awsDynamoDbConfig.SortKeyName))
                {
                    newConfig[_awsDynamoDbConfig.SortKeyName] = createRequest.Key;
                }

                newConfig[nameof(ConfigCreateRequestModel.Value)] = createRequest.Value;
                newConfig[nameof(ConfigCreateRequestModel.Description)] = createRequest.Description;

                await table.PutItemAsync(newConfig);

                _logger.LogInformation("success put config in dynamodb table");
                return new FuncResponse<ConfigCreateResponseModel>
                {
                    Data = new ConfigCreateResponseModel
                    {
                        Key = createRequest.Key,
                        Description = createRequest.Description,
                        Value = createRequest.Value
                    }
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "failed to insert config in dynamodb table");
                return new FuncResponse<ConfigCreateResponseModel>
                {
                    ErrorMessage = "failed to insert config in dynamodb table"
                };
            }
        }

        public async Task<FuncResponse<ConfigDeleteResponseModel>> Delete(ConfigDeleteRequestModel deleteRequest)
        {
            try
            {
                _logger.LogInformation($"Start DeleteConfig w. data: {deleteRequest.ToJson()}");
                if (String.IsNullOrWhiteSpace(_awsDynamoDbConfig.TableName))
                {
                    throw new Exception("tablename in aws dynamodb config is not set");
                }

                if (String.IsNullOrWhiteSpace(_awsDynamoDbConfig.PartitionKeyName))
                {
                    throw new Exception("partition key in aws dynamodb config is not set");
                }

                var table = Table.LoadTable(_amazonDynamoDBClient, _awsDynamoDbConfig.TableName);

                Document document;
                if (!String.IsNullOrWhiteSpace(_awsDynamoDbConfig.SortKeyName))
                {
                    document = await table.DeleteItemAsync(new Primitive
                    {
                        Type = DynamoDBEntryType.String,
                        Value = deleteRequest.Key,
                    }, new Primitive
                    {
                        Type = DynamoDBEntryType.String,
                        Value = deleteRequest.Key
                    });
                }
                else
                {
                    document = await table.DeleteItemAsync(new Primitive
                    {
                        Type = DynamoDBEntryType.String,
                        Value = deleteRequest.Key,
                    });
                }

                _logger.LogInformation("success delete config from dynamodb table");
                return new FuncResponse<ConfigDeleteResponseModel>
                {
                    Data = new ConfigDeleteResponseModel
                    {
                        Key = deleteRequest.Key,
                    },
                };

            }
            catch(Exception e)
            {
                _logger.LogError(e, "failed delete config from dynamodb table");
                return new FuncResponse<ConfigDeleteResponseModel>
                {
                    ErrorMessage = "failed delete config from dynamodb table"
                };
            }
        }

        public async Task<FuncResponse<ConfigGetResponseModel>> Get(ConfigGetRequestModel getRequest)
        {
            try
            {
                _logger.LogInformation($"Start GetConfig w. data: {getRequest.ToJson()}");
                if (String.IsNullOrWhiteSpace(_awsDynamoDbConfig.TableName))
                {
                    throw new Exception("tablename in aws dynamodb config is not set");
                }

                if (String.IsNullOrWhiteSpace(_awsDynamoDbConfig.PartitionKeyName))
                {
                    throw new Exception("partition key in aws dynamodb config is not set");
                }

                var table = Table.LoadTable(_amazonDynamoDBClient, _awsDynamoDbConfig.TableName);

                Document document;
                if (!String.IsNullOrWhiteSpace(_awsDynamoDbConfig.SortKeyName))
                {
                    document = await table.GetItemAsync(new Primitive
                    {
                        Type = DynamoDBEntryType.String,
                        Value = getRequest.Key,
                    }, new Primitive
                    {
                        Type = DynamoDBEntryType.String,
                        Value = getRequest.Key
                    });
                }
                else
                {
                    document = await table.GetItemAsync(new Primitive
                    {
                        Type = DynamoDBEntryType.String,
                        Value = getRequest.Key,
                    });
                }

                if (document == null)
                {
                    throw new Exception("document is null, not found");
                }

                _logger.LogInformation("success get config from dynamodb");
                return new FuncResponse<ConfigGetResponseModel>
                {
                    Data = new ConfigGetResponseModel
                    {
                        Key = getRequest.Key,
                        Value = document[nameof(ConfigCreateRequestModel.Value)],
                        Description = document[nameof(ConfigCreateRequestModel.Description)]
                    }
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, "failed get config");
                return new FuncResponse<ConfigGetResponseModel>
                {
                    ErrorMessage = "failed get config"
                };
            }
        }
    }
}
