using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ReactFullStackDemo.Api.Domain;

public sealed class Post
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Markdown { get; set; } = string.Empty;
    public PostStatus Status { get; set; } = PostStatus.Draft;
    public List<string> Tags { get; set; } = new();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public DateTimeOffset? PublishedAt { get; set; }
}
