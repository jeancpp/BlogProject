using Blog.Data;
using Blog.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Repositories
{
    public class BlogPostLikeRepository : IBlogPostLikeRepository
    {
        private readonly BlogDbContext blogdbContext;

        public BlogPostLikeRepository(BlogDbContext blogDbContext) {
            this.blogdbContext = blogDbContext;
        }

        public async Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike)
        {
            await blogdbContext.BlogPostLike.AddAsync(blogPostLike);
            await blogdbContext.SaveChangesAsync();
            return blogPostLike;
        }

        public async Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId)
        {
            return await blogdbContext.BlogPostLike.Where(x => x.BlogPostId == blogPostId).ToListAsync();
        }

        public async Task<int> GetTotalLikes(Guid blogPostId)
        {
           return await blogdbContext.BlogPostLike.CountAsync(x => x.BlogPostId == blogPostId);
        }
    }
}
