using ReactFullStackDemo.Api.Domain;

namespace ReactFullStackDemo.Api.Data;

public interface IPostRepository
{
    Task<Post?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Post>> ListAsync(PostStatus? status, int skip, int take, CancellationToken cancellationToken);
    Task<Post> InsertAsync(Post post, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(Post post, CancellationToken cancellationToken);
}
