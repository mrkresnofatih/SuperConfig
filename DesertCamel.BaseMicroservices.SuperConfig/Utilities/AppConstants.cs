namespace DesertCamel.BaseMicroservices.SuperConfig.Utilities
{
    public class AppConstants
    {
        public class ConfigKeys
        {
            public const string SELECTED_PROVIDER = "ConfigProviders:Selected";

            public const string POSTGRES_DB_CONN_STRING = "ConfigProviders:Options:PostgreSql";
            public const string MYSQL_DB_CONN_STRING = "ConfigProviders:Options:MySql";
            public const string SQLSERVER_DB_CONN_STRING = "ConfigProviders:Options:SqlServer";
        }

        public class ProviderTypes
        {
            public const string POSTGRES = "PostgreSql";
            public const string AWS_DYNAMODB = "AWSDynamoDb";
            public const string GOOGLE_FIRESTORE = "GoogleFirestore";
        }
    }
}
