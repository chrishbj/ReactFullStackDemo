using ReactFullStackDemo.Api.Data;
using ReactFullStackDemo.Api.Domain;
using ReactFullStackDemo.Api.Dtos;

namespace ReactFullStackDemo.Api.Services;

public sealed class PostService : IPostService
{
    private readonly IPostRepository _repository;

    public PostService(IPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<ServiceResult<Post>> CreateAsync(CreatePostRequest request, CancellationToken cancellationToken)
    {
        var errors = PostValidation.ValidateCreate(request);
        if (errors.Count > 0)
        {
            return ServiceResult<Post>.Validation(errors);
        }

        var now = DateTimeOffset.UtcNow;
        var status = request.Status ?? PostStatus.Draft;
        var post = new Post
        {
            Title = request.Title.Trim(),
            Slug = SlugGenerator.FromTitle(request.Title),
            Markdown = request.Markdown.Trim(),
            Status = status,
            Tags = request.Tags?.Where(tag => !string.IsNullOrWhiteSpace(tag))
                .Select(tag => tag.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList() ?? new List<string>(),
            CreatedAt = now,
            UpdatedAt = now,
            PublishedAt = status == PostStatus.Published ? now : null
        };

        await _repository.InsertAsync(post, cancellationToken);
        return ServiceResult<Post>.Success(post);
    }

    public async Task<ServiceResult<Post>> UpdateAsync(string id, UpdatePostRequest request, CancellationToken cancellationToken)
    {
        var errors = PostValidation.ValidateUpdate(request);
        if (errors.Count > 0)
        {
            return ServiceResult<Post>.Validation(errors);
        }

        var existing = await _repository.GetByIdAsync(id, cancellationToken);
        if (existing is null)
        {
            return ServiceResult<Post>.NotFound();
        }

        var now = DateTimeOffset.UtcNow;
        existing.Title = request.Title.Trim();
        existing.Slug = SlugGenerator.FromTitle(request.Title);
        existing.Markdown = request.Markdown.Trim();
        existing.Tags = request.Tags?.Where(tag => !string.IsNullOrWhiteSpace(tag))
            .Select(tag => tag.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList() ?? new List<string>();

        if (existing.Status != request.Status && request.Status == PostStatus.Published)
        {
            existing.PublishedAt ??= now;
        }

        existing.Status = request.Status;
        existing.UpdatedAt = now;

        var updated = await _repository.UpdateAsync(existing, cancellationToken);
        return updated
            ? ServiceResult<Post>.Success(existing)
            : ServiceResult<Post>.NotFound();
    }

    public Task<Post?> GetByIdAsync(string id, CancellationToken cancellationToken)
        => _repository.GetByIdAsync(id, cancellationToken);

    public Task<IReadOnlyList<Post>> ListAsync(PostStatus? status, int skip, int take, CancellationToken cancellationToken)
        => _repository.ListAsync(status, skip, take, cancellationToken);
}
