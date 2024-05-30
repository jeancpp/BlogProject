using Blog.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models.ViewModels
{
    public class AddBlogPostsRequest
    {
        [Required]
        public string Heading { get; set; }
        [Required]
        public string PageTitle { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string ShortDescription { get; set; }
        [Required]
        public string FeaturedImageUrl { get; set; }
        [Required]
        public string UrlHandle { get; set; }
        [Required]
        public DateTime PublishedDate { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public bool Visible { get; set; }

        //Display created tags
        [Required]
        public IEnumerable<SelectListItem> Tags { get; set; }
        public string[] SelectedTags { get; set; } = Array.Empty<string>();
    }
}
