using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Controllers
{
    // Defines the AdminTagsController, which inherits from the base Controller class.
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository tagRepository;

        public AdminTagsController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }
        // ---------------------- CREATE FUNCTIONALITY ----------------------
        // CREATE FUNCTIONALITY - Adding the tag in these 2 action methods
        // Handles HTTP GET requests to display the "Add" tag form.
        // Add page and show
        [HttpGet]
        public IActionResult Add()
        {
            // Returns the "Add" view, which contains the tag creation form.
            return View();
        }
        // Handles HTTP POST requests to capture form data and save the tag to the database.
        [HttpPost] // Marks this method as handling POST requests.
        [ActionName("Add")] // Ensures this method is mapped to the "Add" action.
        // Accepts user input from the form.
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            // Mapping AddTagRequest (DTO) to Tag (domain model).

            var tag = new Tag
            {
                // Assigns the name entered by the user.
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName  // Assigns the display name entered by the user.
            };
            // Adds a new tag entity to the database using the tagRepository,  
            // which is an instance of a repository that manages Tag entities.  
            // Calls the AddAsync method, which is responsible for inserting a new record into the database.  
            // Uses await to asynchronously wait for the operation to complete before proceeding.  
            // This prevents blocking the main thread and improves performance.  
            await tagRepository.AddAsync(tag);

            // Redirects the user to the "List" page after successfully saving the tag.
            return RedirectToAction("List");
           
        }
        // READ FUNCTIONALITY
        // Handles HTTP GET requests to retrieve and display the list of tags from the database.
        // Create new page where I can display the list of tags coming from the database
        // Specifies that this method handles HTTP GET requests.
        [HttpGet] // Marks this method as handling GET requests.
        [ActionName("List")] // Maps this action method to the "List" route.
        public async Task<IActionResult> List()
        {
            // Retrieves all tags from the database and converts them into a list.
            var tags = await tagRepository.GetAllAsync();
            // Passes the retrieved tags to the "List" view for display.
            return View(tags);
        }
        //READ Fuctionality
        // Create get method
        [HttpGet] // Specifies that this method handles HTTP GET requests.
        // Parameter has to match the name of the route created
        // Accepts a tag ID as a parameter from the URL.
        public async Task<IActionResult> Edit(Guid id) 
        {
           var tag = await tagRepository.GetAsync(id);

            // If the tag exists, prepare an object for the view.
            // display tag into edit page
            if (tag != null) 
            {
                // Create an EditTagRequest object and populate it with the tag's existing details.
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id, // Assigns the ID.
                    Name = tag.Name, // Assigns the name.
                    DisplayName = tag.DisplayName // Assigns the display name.
                };
                // Pass the populated EditTagRequest object to the "Edit" view.
                return View(editTagRequest);

            }
            // If the tag is not found, return the view with null (likely showing an error message).
            return View(null);
        }
        // UPDATE FUNCTIONALIY
        [HttpPost] // Specifies that this method handles HTTP POST requests.
        // Accepts an EditTagRequest object from the form submission.
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest) 
        {
            // Create a new Tag object using values from the editTagRequest
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };
           
            var updatedTag = await tagRepository.UpdateAsync(tag);

            if (updatedTag != null)
            {
                // Show success notification
            }
            else
            {
                // Show error notification
            }

                // If the tag does not exist, still redirect back to the Edit page.
                // Show error notification
                return RedirectToAction("Edit", new { id = editTagRequest.Id });  
        }

        // DELETE FUNCTIONALITY
        // Specifies that this action method will handle POST requests
        [HttpPost]
        // Defines the Delete action method, accepting an EditTagRequest object as a parameter
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest) 
        {

            var deletedTag = await tagRepository.DeleteAsync(editTagRequest.Id);

            if (deletedTag != null)
            {
                // Show success notification
                return RedirectToAction("List");
            }
            
            // Show and error notification
            // If the tag wasn't found, redirects to the "Edit" action with the same ID, indicating an error occurred
            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }

    }
}
