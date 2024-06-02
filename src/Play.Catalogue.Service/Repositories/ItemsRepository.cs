using MongoDB.Driver;
using Play.Catalogue.Service.Entities;

namespace Play.Catalogue.Service.Repositories;

public class ItemsRepository
{
    private const string COLLECTION_NAME = "items";
    private readonly IMongoCollection<Item> _dbCollection;
    private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

    public ItemsRepository()
    {
        var mongoClient = new MongoClient("mongodb://localhost:27017");
        var mongoDatabase = mongoClient.GetDatabase("Catalog");
        _dbCollection = mongoDatabase.GetCollection<Item>(COLLECTION_NAME);
    }

    public async Task<IReadOnlyCollection<Item>> GetAllAsync()
    {
        return await _dbCollection.Find(filterBuilder.Empty).ToListAsync();
    }

    public async Task<Item> GetByIdAsync(Guid id)
    {
        FilterDefinition<Item> filter = filterBuilder.Eq(entity => entity.Id, id);
        return await _dbCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Item entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        await _dbCollection.InsertOneAsync(entity);
    }

    public async Task UpdateAsync(Item entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        FilterDefinition<Item> filter = filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
        await _dbCollection.ReplaceOneAsync(filter, entity);
    }

    public async Task RemoveAsync(Guid id)
    {
        FilterDefinition<Item> filter = filterBuilder.Eq(entity => entity.Id, id);
        await _dbCollection.DeleteOneAsync(filter);
    }
}