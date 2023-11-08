using BlogService.Core.Services.Interfaces;
using BlogService.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PostsController : ControllerBase
    {
        private readonly IBlogPostService _blogPostService;

        public PostsController(IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }

        [HttpPost]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> CreatePost([FromBody] BlogPostDTO postDTO)
        {
            try
            {
                var createdPost = await _blogPostService.CreatePostAsync(postDTO);
                return CreatedAtAction(nameof(CreatePost), new { id = createdPost.Id }, createdPost);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            try
            {
                var post = await _blogPostService.GetPostByIdAsync(id);
                if (post == null)
                {
                    return NotFound($"No Post Found with Id : {id}");
                }
                return Ok(post);
            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            try
            {
                var result = await _blogPostService.GetAllBlogsAsync();

                if(result == null)
                {
                    return NotFound("No Blogs Found");
                }
                return Ok(result);
            }
            catch(Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
