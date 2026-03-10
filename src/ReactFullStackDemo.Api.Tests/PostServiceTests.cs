using ReactFullStackDemo.Api.Domain;
using ReactFullStackDemo.Api.Dtos;
using ReactFullStackDemo.Api.Services;
using ReactFullStackDemo.Api.Tests.Fakes;

namespace ReactFullStackDemo.Api.Tests;

public sealed class PostServiceTests
{
    [Fact]
    public async Task CreateAsync_DefaultsToDraftAndSetsSlug()
    {
        var service = CreateService();
        var request = new CreatePostRequest("Hello World", "content", null, new[] { "dev", "Dev" });

        var result = await service.CreateAsync(request, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(PostStatus.Draft, result.Value!.Status);
        Assert.Equal("hello-world", result.Value.Slug);
        Assert.Single(result.Value.Tags);
    }

    [Fact]
    public async Task UpdateAsync_PublishesAndSetsPublishedAt()
    {
        var service = CreateService();
        var createResult = await service.CreateAsync(
            new CreatePostRequest("Intro", "content", PostStatus.Draft, null),
            CancellationToken.None);

        var updateResult = await service.UpdateAsync(
            createResult.Value!.Id,
            new UpdatePostRequest("Intro Updated", "content", PostStatus.Published, null),
            CancellationToken.None);

        Assert.True(updateResult.IsSuccess);
        Assert.Equal(PostStatus.Published, updateResult.Value!.Status);
        Assert.NotNull(updateResult.Value.PublishedAt);
        Assert.Equal("intro-updated", updateResult.Value.Slug);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNotFoundForUnknownId()
    {
        var service = CreateService();

        var updateResult = await service.UpdateAsync(
            "missing-id",
            new UpdatePostRequest("Intro", "content", PostStatus.Draft, null),
            CancellationToken.None);

        Assert.True(updateResult.IsNotFound);
    }

    private static PostService CreateService()
    {
        var repository = new InMemoryPostRepository();
        return new PostService(repository);
    }
}
