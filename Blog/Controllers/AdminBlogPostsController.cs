using Blog.Models.Domain;
using Blog.Models.ViewModels;
using Blog.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IBlogPostRepository blogPostRepository;

        public AdminBlogPostsController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
        {
            this.tagRepository = tagRepository;
            this.blogPostRepository = blogPostRepository;

        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var tags = await tagRepository.GetAllAsync();

            var model = new AddBlogPostsRequest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.DisplayName, Value = x.Id.ToString() })
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostsRequest addBlogPostsRequest)
        {
            if (ModelState.IsValid == false)
            {
                return View();
            }
            var blogPost = new BlogPost
            {
                Heading = addBlogPostsRequest.Heading,
                PageTitle = addBlogPostsRequest.PageTitle,
                Content = addBlogPostsRequest.Content,
                ShortDescription = addBlogPostsRequest.ShortDescription,
                FeaturedImageUrl = addBlogPostsRequest.FeaturedImageUrl,
                UrlHandle = addBlogPostsRequest.UrlHandle,
                PublishedDate = addBlogPostsRequest.PublishedDate,
                Author = addBlogPostsRequest.Author,
                Visible = addBlogPostsRequest.Visible,
            };

            var SelectedTags = new List<Tag>();
            foreach (var selectedtag in addBlogPostsRequest.SelectedTags)
            {
                var selectedTagId = Guid.Parse(selectedtag);
                var existingTag = await tagRepository.GetAsync(selectedTagId);

                if (existingTag != null)
                {
                    SelectedTags.Add(existingTag);
                }
            }
            blogPost.Tags = SelectedTags;
            await blogPostRepository.AddAsync(blogPost);

            return RedirectToAction("Add");
        }

        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List()
        {
            var blogPosts = await blogPostRepository.GetAllAsync();
            return View(blogPosts);
        }

        [HttpGet]
        [ActionName("Edit")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var blogPost = await blogPostRepository.GetAsync(id);
            var tagsDomainModel = await tagRepository.GetAllAsync();
            if (blogPost != null)
            {
                var editBlogRequest = new EditBlogPostRequest
                {
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    PageTitle = blogPost.PageTitle,
                    Content = blogPost.Content,
                    ShortDescription = blogPost.ShortDescription,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    PublishedDate = blogPost.PublishedDate,
                    Author = blogPost.Author,
                    Visible = blogPost.Visible,
                    Tags = tagsDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()
                };
                return View(editBlogRequest);
            }
            return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            var blogPostDomainModel = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                ShortDescription = editBlogPostRequest.ShortDescription,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                UrlHandle = editBlogPostRequest.UrlHandle,
                PublishedDate = editBlogPostRequest.PublishedDate,
                Author = editBlogPostRequest.Author,
                Visible = editBlogPostRequest.Visible,
            };
            var selectedTags = new List<Tag>();
            foreach (var selectedTag in editBlogPostRequest.SelectedTags)
            {
                if (Guid.TryParse(selectedTag, out var tag))
                {
                    var foundTag = await tagRepository.GetAsync(tag);
                    if (foundTag != null)
                    {
                        selectedTags.Add(foundTag);
                    }
                }
            }

            blogPostDomainModel.Tags= selectedTags;
            var updatedBlog = await blogPostRepository.UpdateAsync(blogPostDomainModel);

            if (updatedBlog != null)
            {
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit");

        }
        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
            var deletedBlogPost = await blogPostRepository.DeleteAsync(editBlogPostRequest.Id);
            if( deletedBlogPost != null)
            {
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new {id = editBlogPostRequest.Id});
        }
    }
}
