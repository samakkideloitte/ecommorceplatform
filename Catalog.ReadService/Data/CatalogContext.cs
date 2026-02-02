using MongoDB.Driver;
using Catalog.ReadService.Models;

namespace Catalog.ReadService.Data
{
    public class CatalogContext
    {
        public IMongoCollection<CatalogItem> Items { get; }
        public IMongoCollection<Models.AdminKey> AdminKeys { get; }


        public CatalogContext(IConfiguration config)
        {
            var client = new MongoClient(config["MongoDb:ConnectionString"]);
            var database = client.GetDatabase(config["MongoDb:Database"]);
            Items = database.GetCollection<CatalogItem>(config["MongoDb:Collection"]);
            AdminKeys = database.GetCollection<Models.AdminKey>("AdminKeys");
        }
    }
}
