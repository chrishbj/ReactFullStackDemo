using ReactFullStackDemo.Api.Domain;
using ReactFullStackDemo.Api.Dtos;
using ReactFullStackDemo.Api.Services;

namespace ReactFullStackDemo.Api.Tests;

public sealed class PostValidationTests
{
    [Fact]
    public void ValidateCreate_RequiresTitleAndMarkdown()
    {
        var request = new CreatePostRequest("", "", null, null);

        var errors = PostValidation.ValidateCreate(request);

        Assert.True(errors.ContainsKey("title"));
        Assert.True(errors.ContainsKey("markdown"));
    }

    [Fact]
    public void ValidateUpdate_RequiresTitleAndMarkdown()
    {
        var request = new UpdatePostRequest("", "", PostStatus.Draft, null);

        var errors = PostValidation.ValidateUpdate(request);

        Assert.True(errors.ContainsKey("title"));
        Assert.True(errors.ContainsKey("markdown"));
    }
}
