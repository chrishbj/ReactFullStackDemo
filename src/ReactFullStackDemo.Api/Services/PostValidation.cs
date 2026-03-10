using ReactFullStackDemo.Api.Dtos;

namespace ReactFullStackDemo.Api.Services;

public static class PostValidation
{
    private const int MaxTitleLength = 200;
    private const int MaxTags = 10;

    public static IReadOnlyDictionary<string, string[]> ValidateCreate(CreatePostRequest request)
    {
        var errors = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

        AddTitleErrors(request.Title, errors);
        AddMarkdownErrors(request.Markdown, errors);
        AddTagsErrors(request.Tags, errors);

        return errors;
    }

    public static IReadOnlyDictionary<string, string[]> ValidateUpdate(UpdatePostRequest request)
    {
        var errors = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

        AddTitleErrors(request.Title, errors);
        AddMarkdownErrors(request.Markdown, errors);
        AddTagsErrors(request.Tags, errors);

        return errors;
    }

    private static void AddTitleErrors(string title, IDictionary<string, string[]> errors)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            errors["title"] = new[] { "Title is required." };
            return;
        }

        if (title.Length > MaxTitleLength)
        {
            errors["title"] = new[] { $"Title cannot exceed {MaxTitleLength} characters." };
        }
    }

    private static void AddMarkdownErrors(string markdown, IDictionary<string, string[]> errors)
    {
        if (string.IsNullOrWhiteSpace(markdown))
        {
            errors["markdown"] = new[] { "Markdown content is required." };
        }
    }

    private static void AddTagsErrors(IReadOnlyList<string>? tags, IDictionary<string, string[]> errors)
    {
        if (tags is null)
        {
            return;
        }

        if (tags.Count > MaxTags)
        {
            errors["tags"] = new[] { $"Tags cannot exceed {MaxTags} items." };
        }
    }
}
