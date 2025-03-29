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

        // Create get method
        [HttpGet]
        // Parameter has to match the name of the route created
        public IActionResult Edit(Guid id) 
        { 
            // Use bloggieDbContext to connect to database to read details to display on screen and allow user to update or edit details
            //1st method var tag = bloggieDbContext.Tags.Find(id);
            //2nd method will find the tag using the id and the first one it finds it will give it back
            var tag = bloggieDbContext.Tags.FirstOrDefault(x => x.Id == id);

            // display tag into edit page
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
        [HttpPost] // Specifies that this method handles HTTP POST requests.
        // Accepts an EditTagRequest object from the form submission.
        public IActionResult Edit(EditTagRequest editTagRequest) 
        {
            // Create a new Tag object using values from the editTagRequest
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };
            // Find the existing tag in the database using the provided ID.
            var existingTag = bloggieDbContext.Tags.Find(tag.Id);
            // It queries the bloggieDbContext.Tags table to find a tag with the given Id.
            // Check if the existing tag was found
            if (existingTag != null)
            {
                // Update the existing tag's Name and DisplayName with the new values.
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                // Save the updated tag details to the database.
                bloggieDbContext.SaveChanges();

                // Redirect back to the Edit page for the given tag ID.
                // Show success notification
                return RedirectToAction("Edit", new { id = editTagRequest.Id });
               
            }
            // If the tag does not exist, still redirect back to the Edit page.
            // Show error notification
            return RedirectToAction("Edit", new { id = editTagRequest.Id });  
        }

    }
}
