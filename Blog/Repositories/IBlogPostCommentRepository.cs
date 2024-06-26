﻿using Blog.Models.Domain;

namespace Blog.Repositories
{
    public interface IBlogPostCommentRepository
    {
        Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment);
        Task<IEnumerable<BlogPostComment>> GetCommentsbyBlogIdAsync(Guid blogPostId);
    }
}
