using BlogService.Core.Services.Interfaces;
using BlogService.DataAccess.Respositories.Interfaces;
using BlogService.Models;
using BlogService.Shared.DTOs;
using System.Linq.Expressions;

namespace BlogService.Core.Services
{
    public class BlogPostService : IBlogPostService
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly ICacheService _cacheService;

        public BlogPostService(IBlogPostRepository blogPostRepository, ICacheService cacheService)
        {
            _blogPostRepository = blogPostRepository;
            _cacheService = cacheService;
        }

        public async Task<BlogPost> CreatePostAsync(BlogPostDTO post)
        {
            var newPost = new BlogPost
            {
                Title = post.Title,
                Content = post.Content,
                PublishedDate = DateTime.UtcNow,
                LastUpdatedDate = DateTime.UtcNow,
                Comments = new List<Comment>()
            };

            var createdPost = await _blogPostRepository.CreateAsync(newPost);

            string cacheKey = $"post{createdPost.Id}";
            var expiryTime = DateTimeOffset.Now.AddSeconds(60);

            _cacheService.SetDataAsync<BlogPost>(cacheKey, createdPost, expiryTime);

            return createdPost;
        }

        public async Task<BlogPost> GetPostByIdAsync(int postId)
        {
            var cachedPostData = await _cacheService.GetDataAsync<BlogPost>($"post{postId}");
            if(cachedPostData != null)
            {
                return cachedPostData;
            }

            var post = await _blogPostRepository.FindByIdAsync(postId);

            string cacheKey = $"post{post.Id}";
            var expiryTime = DateTimeOffset.Now.AddSeconds(60);

            _cacheService.SetDataAsync<BlogPost>(cacheKey, post, expiryTime);

            return post;
        }

        public async Task<IEnumerable<BlogPost>> GetAllBlogsAsync(Expression<Func<BlogPost, bool>>? filter = null, string? includeProperties = null)
        {
            string cacheKey = "allPosts";
            var cachedData = await _cacheService.GetDataAsync<IEnumerable<BlogPost>>(cacheKey);

            if(cachedData != null)
                return cachedData;

            var allPosts = await _blogPostRepository.GetAllAsync(includeProperties : nameof(BlogPost.Comments));
            var expiryTime = DateTimeOffset.Now.AddSeconds(60);
            _cacheService.SetDataAsync(cacheKey, allPosts, expiryTime);

            return allPosts;

        }


    }
}
