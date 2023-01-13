using MongoDB.Bson;

namespace MongoDBRepository.Model
{
    public interface IMongoDBBaseEntity
    {
        DateTime EntryDate { get; set; }
        ObjectId Id { get; set; }
    }
}