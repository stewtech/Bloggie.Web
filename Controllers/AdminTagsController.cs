using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
   
    public class AdminTagsController : Controller
    {
        private readonly BloggieDbContext bloggieDbContext;

        public AdminTagsController(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }
        // Create Functionality - Adding the tag in these 2 action methods
        // Add page and show
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        // Capture details and post the tag to save to the database
        [HttpPost]
        [ActionName("Add")]
        public IActionResult Add(AddTagRequest addTagRequest)
        {
            // Mapping AddTagRequest to Tag domain model

            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };

            bloggieDbContext.Tags.Add(tag);
            bloggieDbContext.SaveChanges();
            return RedirectToAction("List");
            // After saving the user will get redirected to the list page.
        }

        // Create new page where I can display the list of tags coming from the database
        [HttpGet]
        [ActionName("List")]
        public IActionResult List()
        {
            // use dbcontext to read the tags
            var tags = bloggieDbContext.Tags.ToList();

            return View(tags);
        }
    }
}
