using MongoDB.Bson;

namespace MongoDBRepository.Model;

public abstract class MongoDBBaseEntity : IMongoDBBaseEntity
{

    public ObjectId Id { get; set; }

    public DateTime EntryDate { get; set; } = DateTime.UtcNow;
}
