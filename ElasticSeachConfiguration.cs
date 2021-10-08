using Nest;
using System; 
using System.Configuration; 

namespace ElasticSearch.Connector
{
    public sealed class ElasticSeachConfiguration
    {
        private static readonly string elasticUrl = ConfigurationManager.AppSettings["ElasticUrl"];
        private static readonly string elasticUserName = ConfigurationManager.AppSettings["ElasticUserName"];
        private static readonly string elasticPassword = ConfigurationManager.AppSettings["ElasticPassword"];

        private static readonly Uri elasticUri = new Uri(elasticUrl);
        private static readonly ConnectionSettings connectionSettings = new ConnectionSettings(elasticUri)
        .DisableDirectStreaming().BasicAuthentication(elasticUserName, elasticPassword);
        private static readonly ElasticClient instance = new ElasticClient(connectionSettings);
        static ElasticSeachConfiguration() { }
        private ElasticSeachConfiguration() { }

        public static IElasticClient ESContext => instance;
    }

}
