using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ReactFullStackDemo.Api.Domain;

namespace ReactFullStackDemo.Api.Data;

public sealed class PostRepository : IPostRepository
{
    private readonly IMongoCollection<Post> _collection;

    public PostRepository(IMongoClient mongoClient, IOptions<MongoOptions> options)
    {
        var settings = options.Value;
        var database = mongoClient.GetDatabase(settings.Database);
        _collection = database.GetCollection<Post>(settings.PostsCollection);
    }

    public async Task<Post?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return await _collection.Find(post => post.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Post>> ListAsync(PostStatus? status, int skip, int take, CancellationToken cancellationToken)
    {
        var filter = status is null
            ? Builders<Post>.Filter.Empty
            : Builders<Post>.Filter.Eq(post => post.Status, status.Value);

        return await _collection.Find(filter)
            .SortByDescending(post => post.UpdatedAt)
            .Skip(skip)
            .Limit(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<Post> InsertAsync(Post post, CancellationToken cancellationToken)
    {
        await _collection.InsertOneAsync(post, cancellationToken: cancellationToken);
        return post;
    }

    public async Task<bool> UpdateAsync(Post post, CancellationToken cancellationToken)
    {
        var result = await _collection.ReplaceOneAsync(
            existing => existing.Id == post.Id,
            post,
            cancellationToken: cancellationToken);

        return result.IsAcknowledged && result.ModifiedCount > 0;
    }
}
