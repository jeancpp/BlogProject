using Blog.Data;
using Blog.Models.Domain;
using Blog.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Blog.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly BlogDbContext blogDbContext;
        public TagRepository(BlogDbContext blogDbContext)
        {
            this.blogDbContext = blogDbContext;
        }


        public async Task<IEnumerable<Tag>> GetAllAsync(string? searchQuery,
            string? sortBy, string? sortDirection, int pageNumber = 1, int pageSize = 100)
        {
            var query = blogDbContext.Tag.AsQueryable();

            if (string.IsNullOrEmpty(searchQuery) == false)
            {
                query = query.Where(x => x.Name.Contains(searchQuery) ||
                x.DisplayName.Contains(searchQuery));
            }

            if (string.IsNullOrEmpty(sortBy) == false)
            {
                var isDesc = string.Equals(sortDirection, "Desc", StringComparison.OrdinalIgnoreCase);
                if (string.Equals(sortBy, "Name", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.Name): query.OrderBy(x=> x.Name);
                }

                if (string.Equals(sortBy, "DisplayName", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.DisplayName);
                }
            }

            if (string.IsNullOrEmpty(searchQuery) == false)
            {
                query = query.Where(x => x.Name.Contains(searchQuery) ||
                x.DisplayName.Contains(searchQuery));
            }

            var skipResults = (pageNumber - 1) * pageSize;
            query = query.Skip(skipResults).Take(pageSize);
            return await query.ToListAsync();
            //return await blogDbContext.Tag.ToListAsync();
        }
        public async Task<Tag> AddAsync(Tag tag)
        {
            await blogDbContext.Tag.AddAsync(tag);
            await blogDbContext.SaveChangesAsync();

            return tag;
        }

        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var existingtag = await blogDbContext.Tag.FindAsync(id);

            if (existingtag != null)
            {
                blogDbContext.Tag.Remove(existingtag);
                await blogDbContext.SaveChangesAsync();
                return existingtag;
            }
            return null;
        }


        public Task<Tag?> GetAsync(Guid id)
        {
            return blogDbContext.Tag.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var existingtag = await blogDbContext.Tag.FindAsync(tag.Id);
            if (existingtag != null)
            {
                existingtag.Name = tag.Name;
                existingtag.DisplayName = tag.DisplayName;
                await blogDbContext.SaveChangesAsync();

                return existingtag;
            }
            return null;
        }

        public async Task<int> CountAsync()
        {
           return await blogDbContext.Tag.CountAsync();
        }
    }
}
