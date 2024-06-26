﻿using Blog.Models.Domain;

namespace Blog.Repositories
{
    public interface IBlogPostLikeRepository
    {
        Task<int> GetTotalLikes(Guid blogPostId);

        Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike);

        Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId);

    }
}
