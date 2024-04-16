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

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await blogDbContext.Tag.ToListAsync();
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
    }
}
