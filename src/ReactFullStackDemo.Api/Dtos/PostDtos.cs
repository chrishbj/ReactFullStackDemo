using ReactFullStackDemo.Api.Domain;

namespace ReactFullStackDemo.Api.Dtos;

public sealed record CreatePostRequest(
    string Title,
    string Markdown,
    PostStatus? Status,
    IReadOnlyList<string>? Tags);

public sealed record UpdatePostRequest(
    string Title,
    string Markdown,
    PostStatus Status,
    IReadOnlyList<string>? Tags);

public sealed record PostResponse(
    string Id,
    string Title,
    string Slug,
    string Markdown,
    PostStatus Status,
    IReadOnlyList<string> Tags,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    DateTimeOffset? PublishedAt);

public sealed record PostSummaryResponse(
    string Id,
    string Title,
    string Slug,
    PostStatus Status,
    IReadOnlyList<string> Tags,
    DateTimeOffset UpdatedAt,
    DateTimeOffset? PublishedAt);
