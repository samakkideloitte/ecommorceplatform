
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.ReadService.Models
{
    public class CatalogItem
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
