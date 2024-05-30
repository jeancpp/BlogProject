using Blog.Data;
using Blog.Models.Domain;
using Blog.Models.ViewModels;
using Blog.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository tagRepository;
        public AdminTagsController(ITagRepository tagRepository) 
        {
            this.tagRepository = tagRepository;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            ValidateAddTagRequest(addTagRequest);
            if (ModelState.IsValid == false)
            {
                return View();
            }
            //Mapping AddTAgREquest to Tag domanin model
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName,
            };

            await tagRepository.AddAsync(tag);
            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List(string? searchQuery,
            string? sortBy,
            string? sortDirection,
            int pageSize =3, int pageNumber = 1)
        {

            var totalRecords = await tagRepository.CountAsync();
            var totalPages = Math.Ceiling((decimal)(totalRecords / pageSize));

            if(pageNumber > totalPages)
            {
                pageNumber--;
            }    

            if(pageNumber < 1)
            {
                pageNumber++;
            }

            ViewBag.SearchQuery = searchQuery;
            ViewBag.sortBy = sortBy;
            ViewBag.sortDirection = sortDirection;
            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;

            ViewBag.TotalPages = totalPages;
            var tags = await tagRepository.GetAllAsync(searchQuery, sortBy, sortDirection, pageNumber, pageSize);

             return View(tags);
        }

        [HttpGet]
        [ActionName("Edit")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await tagRepository.GetAsync(id);

            if (tag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };
                return View(editTagRequest);
            }
            return View(null);
        }

        [HttpPost]
        [ActionName("Edit")]
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            ValidateEditTagRequest(editTagRequest);
            if (ModelState.IsValid == false)
            {
                return View();
            }
            //Mapping AddTAgREquest to Tag domanin model
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName,
            };

            var updatedtag = await tagRepository.UpdateAsync(tag);

            if (updatedtag != null)
            {
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new { id = editTagRequest.Id });

        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            var deletedtag = await tagRepository.DeleteAsync(editTagRequest.Id);

            if (deletedtag != null)
            {
                return RedirectToAction("List");
            }


            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }

        private void ValidateAddTagRequest(AddTagRequest addTagRequest)
        {
            if(addTagRequest.Name is not null && addTagRequest.DisplayName is not null)
            {
                if(addTagRequest.DisplayName == addTagRequest.Name)
                {
                    ModelState.AddModelError("DisplayName", "Name cannot be the same as DisplayName");
                }

            }
        }

        private void ValidateEditTagRequest(EditTagRequest editTagRequest)
        {
            if (editTagRequest.Name is not null && editTagRequest.DisplayName is not null)
            {
                if (editTagRequest.DisplayName == editTagRequest.Name)
                {
                    ModelState.AddModelError("DisplayName", "Name cannot be the same as DisplayName");
                }

            }
        }
    }
}
