using MongoDB.Driver;
using MongoDBRepository.Model;
using System.Linq.Expressions;

namespace MongoDBRepository.Repository;

public interface IMongoDbGenericRepository<TEntity> where TEntity : IMongoDBBaseEntity
{

    Task<(int totalPages, IReadOnlyList<TEntity> data)> Pagination(int pageIndex, int pageSize, Expression<Func<TEntity, object>> field);
    List<TEntity> ReadAll();
    Task<List<TEntity>> ReadAllAsync();
    IQueryable<TEntity> AsQueryable();

    IEnumerable<TEntity> FilterBy(Expression<Func<TEntity, bool>> filterExpression);

    IEnumerable<TProjected> FilterBy<TProjected>(
        Expression<Func<TEntity, bool>> filterExpression,
        Expression<Func<TEntity, TProjected>> projectionExpression);

    TEntity FindOne(Expression<Func<TEntity, bool>> filterExpression);

    TProjected FindOne<TProjected>(
        Expression<Func<TEntity, bool>> filterExpression,
        Expression<Func<TEntity, TProjected>> projectionExpression);
    Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filterExpression);

    Task<TProjected> FindOneAsync<TProjected>(
        Expression<Func<TEntity, bool>> filterExpression,
        Expression<Func<TEntity, TProjected>> projectionExpression);
    TEntity FindById(string id);

    Task<TEntity> FindByIdAsync(string id);

    void InsertOne(TEntity document);

    Task InsertOneAsync(TEntity document);

    void InsertMany(ICollection<TEntity> documents);

    Task InsertManyAsync(ICollection<TEntity> documents);

    void UpdateOne(Expression<Func<TEntity, bool>> expression, UpdateDefinition<TEntity> updateDefinition, UpdateOptions? options = default);
    void UpdateMany(Expression<Func<TEntity, bool>> expression, UpdateDefinition<TEntity> updateDefinition, UpdateOptions? options = default);

    Task UpdateOneAsync(Expression<Func<TEntity, bool>> expression, UpdateDefinition<TEntity> updateDefinition, UpdateOptions? options = default);
    Task UpdateManyAsync(Expression<Func<TEntity, bool>> expression, UpdateDefinition<TEntity> updateDefinition, UpdateOptions? options = default);
    void ReplaceOne(TEntity document);

    Task ReplaceOneAsync(TEntity document);

    TEntity FindOneAndDelete(Expression<Func<TEntity, bool>> filterExpression);

    Task<TEntity> FindOneAndDeleteAsync(Expression<Func<TEntity, bool>> filterExpression);
    void DeleteOne(Expression<Func<TEntity, bool>> filterExpression);

    Task DeleteOneAsync(Expression<Func<TEntity, bool>> filterExpression);

    void DeleteMany(Expression<Func<TEntity, bool>> filterExpression);

    Task DeleteManyAsync(Expression<Func<TEntity, bool>> filterExpression);
    Task DeleteByIdAsync(string id);
    void DeleteById(string id);
}