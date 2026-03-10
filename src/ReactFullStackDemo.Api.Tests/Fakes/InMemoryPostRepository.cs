using ReactFullStackDemo.Api.Data;
using ReactFullStackDemo.Api.Domain;

namespace ReactFullStackDemo.Api.Tests.Fakes;

public sealed class InMemoryPostRepository : IPostRepository
{
    private readonly List<Post> _posts = new();

    public Task<Post?> GetByIdAsync(string id, CancellationToken cancellationToken)
        => Task.FromResult(_posts.FirstOrDefault(post => post.Id == id));

    public Task<IReadOnlyList<Post>> ListAsync(PostStatus? status, int skip, int take, CancellationToken cancellationToken)
    {
        var query = _posts.AsEnumerable();
        if (status is not null)
        {
            query = query.Where(post => post.Status == status.Value);
        }

        var results = query
            .OrderByDescending(post => post.UpdatedAt)
            .Skip(skip)
            .Take(take)
            .ToList();

        return Task.FromResult<IReadOnlyList<Post>>(results);
    }

    public Task<Post> InsertAsync(Post post, CancellationToken cancellationToken)
    {
        post.Id = Guid.NewGuid().ToString("N");
        _posts.Add(post);
        return Task.FromResult(post);
    }

    public Task<bool> UpdateAsync(Post post, CancellationToken cancellationToken)
    {
        var index = _posts.FindIndex(existing => existing.Id == post.Id);
        if (index < 0)
        {
            return Task.FromResult(false);
        }

        _posts[index] = post;
        return Task.FromResult(true);
    }
}
