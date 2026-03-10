using Microsoft.AspNetCore.Mvc;
using ReactFullStackDemo.Api.Domain;
using ReactFullStackDemo.Api.Dtos;
using ReactFullStackDemo.Api.Mapping;
using ReactFullStackDemo.Api.Services;

namespace ReactFullStackDemo.Api.Controllers;

[ApiController]
[Route("api/posts")]
public sealed class PostsController : ControllerBase
{
    private readonly IPostService _service;

    public PostsController(IPostService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PostSummaryResponse>>> List(
        [FromQuery] PostStatus? status,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 20,
        CancellationToken cancellationToken = default)
    {
        var posts = await _service.ListAsync(status, Math.Clamp(skip, 0, 10_000), Math.Clamp(take, 1, 200), cancellationToken);
        return Ok(posts.Select(PostMapper.ToSummary));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PostResponse>> GetById(string id, CancellationToken cancellationToken)
    {
        var post = await _service.GetByIdAsync(id, cancellationToken);
        return post is null ? NotFound() : Ok(PostMapper.ToResponse(post));
    }

    [HttpPost]
    public async Task<ActionResult<PostResponse>> Create(CreatePostRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(request, cancellationToken);
        if (!result.IsSuccess)
        {
            return BadRequest(new { errors = result.Errors });
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, PostMapper.ToResponse(result.Value!));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PostResponse>> Update(string id, UpdatePostRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(id, request, cancellationToken);
        if (result.IsNotFound)
        {
            return NotFound();
        }

        if (!result.IsSuccess)
        {
            return BadRequest(new { errors = result.Errors });
        }

        return Ok(PostMapper.ToResponse(result.Value!));
    }
}
