using Blog.Models.Domain;

namespace Blog.Repositories
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllAsync(string? searchQuery = null, string? sortBy = null, string? sortDirection = null,
            int pageSize = 1, int pageNumber = 100);

        Task<Tag?> GetAsync(Guid id);
        Task<Tag> AddAsync(Tag tag);
        Task<Tag?> UpdateAsync(Tag tag);
        Task<Tag?> DeleteAsync(Guid id);

        Task<int> CountAsync();

    }
}
