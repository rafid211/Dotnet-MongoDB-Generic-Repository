using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBRepository.Model;
using MongoDBRepository.Shared;
using System.Linq.Expressions;

namespace MongoDBRepository.Repository;


public class MongoDbGenericRepository<TEntity> : IMongoDbGenericRepository<TEntity> where TEntity : IMongoDBBaseEntity
{
    private readonly IMongoCollection<TEntity> _collection;

    public MongoDbGenericRepository(IMongoDBSettings settings)
    {
        var database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
        _collection = database.GetCollection<TEntity>(MongoDbGenericRepository<TEntity>.GetCollectionName(typeof(TEntity)));
    }

    private static string GetCollectionName(Type documentType)
    {
        string? getCollectionNameFromAttribute = (documentType.GetCustomAttributes(typeof(BsonCollectionAttribute), true)
                                                  .FirstOrDefault() as BsonCollectionAttribute)?.CollectionName;

        return getCollectionNameFromAttribute ?? documentType.Name;

    }
    public virtual async Task<(int totalPages, IReadOnlyList<TEntity> data)> Pagination(int pageIndex, int pageSize, Expression<Func<TEntity, object>> selector)
    {
        var results = await _collection.AggregateByPage(Builders<TEntity>.Filter.Empty,
                                                Builders<TEntity>.Sort.Ascending(selector),
                                                page: pageIndex, pageSize: pageSize);
        return results;

    }
    public virtual IQueryable<TEntity> AsQueryable()
    {
        return _collection.AsQueryable();
    }
    public List<TEntity> ReadAll()
    {
        return _collection.Find(new BsonDocument()).ToList();
    }
    public async Task<List<TEntity>> ReadAllAsync()
    {
        return await _collection.Find(new BsonDocument()).ToListAsync();
    }

    public virtual IEnumerable<TEntity> FilterBy(Expression<Func<TEntity, bool>> filterExpression)
    {
        return _collection.Find(filterExpression).ToEnumerable();
    }

    public virtual IEnumerable<TProjected> FilterBy<TProjected>(
        Expression<Func<TEntity, bool>> filterExpression,
        Expression<Func<TEntity, TProjected>> projectionExpression)
    {
        return _collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
    }

    public virtual TEntity FindOne(Expression<Func<TEntity, bool>> filterExpression)
    {
        return _collection.Find(filterExpression).FirstOrDefault();
    }
    public virtual TProjected FindOne<TProjected>(
        Expression<Func<TEntity, bool>> filterExpression,
        Expression<Func<TEntity, TProjected>> projectionExpression)
    {
        return _collection.Find(filterExpression).Project(projectionExpression).FirstOrDefault();
    }

    public virtual async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filterExpression)
    {
        return await _collection.Find(filterExpression).FirstOrDefaultAsync();
    }

    public virtual async Task<TProjected> FindOneAsync<TProjected>(
        Expression<Func<TEntity, bool>> filterExpression,
        Expression<Func<TEntity, TProjected>> projectionExpression)
    {
        return await _collection.Find(filterExpression).Project(projectionExpression).FirstOrDefaultAsync();
    }

    public virtual TEntity FindById(string id)
    {
        var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, GetObjectId(id));
        return _collection.Find(filter).SingleOrDefault();
    }

    public virtual async Task<TEntity> FindByIdAsync(string id)
    {
        var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, GetObjectId(id));
        return await _collection.Find(filter).SingleOrDefaultAsync();

    }

    public virtual void InsertOne(TEntity document)
    {
        _collection.InsertOne(document);
    }

    public virtual async Task InsertOneAsync(TEntity document)
    {
        await _collection.InsertOneAsync(document);
    }

    public virtual void InsertMany(ICollection<TEntity> documents)
    {
        _collection.InsertMany(documents);
    }

    public virtual async Task InsertManyAsync(ICollection<TEntity> documents)
    {
        await _collection.InsertManyAsync(documents);
    }
    public virtual void UpdateOne(Expression<Func<TEntity, bool>> expression, UpdateDefinition<TEntity> updateDefinition, UpdateOptions? options = default)
    {
        _collection.UpdateOne(expression, updateDefinition, options);
    }
    public virtual void UpdateMany(Expression<Func<TEntity, bool>> expression, UpdateDefinition<TEntity> updateDefinition, UpdateOptions? options = default)
    {
        _collection.UpdateMany(expression, updateDefinition, options);
    }
    public virtual async Task UpdateOneAsync(Expression<Func<TEntity, bool>> expression, UpdateDefinition<TEntity> updateDefinition, UpdateOptions? options = default)
    {
        await _collection.UpdateOneAsync(expression, updateDefinition, options);
    }
    public virtual async Task UpdateManyAsync(Expression<Func<TEntity, bool>> expression, UpdateDefinition<TEntity> updateDefinition, UpdateOptions? options = default)
    {
        await _collection.UpdateManyAsync(expression, updateDefinition, options);
    }

    public virtual void ReplaceOne(TEntity document)
    {
        var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, document.Id);
        _collection.FindOneAndReplace(filter, document);
    }

    public virtual async Task ReplaceOneAsync(TEntity document)
    {
        var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, document.Id);
        await _collection.FindOneAndReplaceAsync(filter, document);
    }

    public virtual TEntity FindOneAndDelete(Expression<Func<TEntity, bool>> filterExpression)
    {
        return _collection.FindOneAndDelete(filterExpression);
    }

    public virtual async Task<TEntity> FindOneAndDeleteAsync(Expression<Func<TEntity, bool>> filterExpression)
    {
        return await _collection.FindOneAndDeleteAsync(filterExpression);
    }

    public virtual void DeleteOne(Expression<Func<TEntity, bool>> filterExpression)
    {
        _collection.DeleteOne(filterExpression);
    }

    public virtual async Task DeleteOneAsync(Expression<Func<TEntity, bool>> filterExpression)
    {
        await _collection.DeleteOneAsync(filterExpression);

    }

    public virtual void DeleteMany(Expression<Func<TEntity, bool>> filterExpression)
    {
        _collection.DeleteMany(filterExpression);
    }

    public virtual async Task DeleteManyAsync(Expression<Func<TEntity, bool>> filterExpression)
    {
        await _collection.DeleteManyAsync(filterExpression);
    }

    public virtual void DeleteById(string id)
    {
        var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, GetObjectId(id));
        _collection.FindOneAndDelete(filter);
    }

    public virtual async Task DeleteByIdAsync(string id)
    {
        var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, GetObjectId(id));
        await _collection.FindOneAndDeleteAsync(filter);
    }

    private static ObjectId GetObjectId(string id) => new ObjectId(id);

}