using ReactFullStackDemo.Api.Domain;
using ReactFullStackDemo.Api.Dtos;

namespace ReactFullStackDemo.Api.Services;

public interface IPostService
{
    Task<ServiceResult<Post>> CreateAsync(CreatePostRequest request, CancellationToken cancellationToken);
    Task<ServiceResult<Post>> UpdateAsync(string id, UpdatePostRequest request, CancellationToken cancellationToken);
    Task<Post?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Post>> ListAsync(PostStatus? status, int skip, int take, CancellationToken cancellationToken);
}
