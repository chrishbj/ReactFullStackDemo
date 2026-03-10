using ReactFullStackDemo.Api.Domain;
using ReactFullStackDemo.Api.Mapping;

namespace ReactFullStackDemo.Api.Tests;

public sealed class PostMapperTests
{
    [Fact]
    public void ToResponse_MapsAllFields()
    {
        var post = new Post
        {
            Id = "1",
            Title = "Title",
            Slug = "title",
            Markdown = "md",
            Status = PostStatus.Published,
            Tags = new List<string> { "a", "b" },
            CreatedAt = DateTimeOffset.UtcNow.AddDays(-1),
            UpdatedAt = DateTimeOffset.UtcNow,
            PublishedAt = DateTimeOffset.UtcNow.AddHours(-2)
        };

        var response = PostMapper.ToResponse(post);

        Assert.Equal(post.Id, response.Id);
        Assert.Equal(post.Title, response.Title);
        Assert.Equal(post.Markdown, response.Markdown);
        Assert.Equal(post.Status, response.Status);
        Assert.Equal(post.Tags, response.Tags);
        Assert.Equal(post.PublishedAt, response.PublishedAt);
    }
}
