using ReactFullStackDemo.Api.Domain;
using ReactFullStackDemo.Api.Dtos;

namespace ReactFullStackDemo.Api.Mapping;

public static class PostMapper
{
    public static PostResponse ToResponse(Post post) => new(
        post.Id,
        post.Title,
        post.Slug,
        post.Markdown,
        post.Status,
        post.Tags,
        post.CreatedAt,
        post.UpdatedAt,
        post.PublishedAt);

    public static PostSummaryResponse ToSummary(Post post) => new(
        post.Id,
        post.Title,
        post.Slug,
        post.Status,
        post.Tags,
        post.UpdatedAt,
        post.PublishedAt);
}
