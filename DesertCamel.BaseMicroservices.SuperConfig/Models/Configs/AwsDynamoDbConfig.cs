namespace DesertCamel.BaseMicroservices.SuperConfig.Models.Configs
{
    public class AwsDynamoDbConfig
    {
        public const string AwsDynamoDbSection = "ConfigProviders:Options:AWSDynamoDb";

        public string TableName { get; set; }
        public string Region { get; set; }
        public string PartitionKeyName { get; set; }
        public string SortKeyName { get; set; }
    }
}
