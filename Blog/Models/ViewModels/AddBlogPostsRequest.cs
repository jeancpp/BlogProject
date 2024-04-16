﻿using Blog.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.Models.ViewModels
{
    public class AddBlogPostsRequest
    {

            public string Heading { get; set; }
            public string PageTitle { get; set; }
            public string Content { get; set; }
            public string ShortDescription { get; set; }
            public string FeaturedImageUrl { get; set; }
            public string UrlHandle { get; set; }
            public DateTime PublishedDate { get; set; }
            public string Author { get; set; }
            public bool Visible { get; set; }

        //Display created tags

        public IEnumerable<SelectListItem> Tags { get; set; }

        public string [] SelectedTags { get; set; } = Array.Empty<string>();
    }
}
