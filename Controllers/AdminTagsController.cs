using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    // Defines the AdminTagsController, which inherits from the base Controller class.
    public class AdminTagsController : Controller
    {
        // Declares a private readonly field for BloggieDbContext to interact with the database.
        private readonly BloggieDbContext bloggieDbContext;
        // Constructor to initialize the BloggieDbContext instance via dependency injection.

        public AdminTagsController(BloggieDbContext bloggieDbContext)
        {
            // Assigns the injected database context to the private field.
            this.bloggieDbContext = bloggieDbContext;
        }
        // ---------------------- CREATE FUNCTIONALITY ----------------------
        // Create Functionality - Adding the tag in these 2 action methods
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
        public IActionResult Add(AddTagRequest addTagRequest)
        {
            // Mapping AddTagRequest (DTO) to Tag (domain model).

            var tag = new Tag
            {
                // Assigns the name entered by the user.
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName  // Assigns the display name entered by the user.
            };
            // Adds the new tag to the database.
            bloggieDbContext.Tags.Add(tag);
            // Saves changes to the database to persist the new tag.
            bloggieDbContext.SaveChanges();
            // Redirects the user to the "List" page after successfully saving the tag.
            return RedirectToAction("List");
           
        }
        // Handles HTTP GET requests to retrieve and display the list of tags from the database.
        // Create new page where I can display the list of tags coming from the database
        // Specifies that this method handles HTTP GET requests.
        [HttpGet] // Marks this method as handling GET requests.
        [ActionName("List")] // Maps this action method to the "List" route.
        public IActionResult List()
        {
            // Retrieves all tags from the database and converts them into a list.
            var tags = bloggieDbContext.Tags.ToList();
            // Passes the retrieved tags to the "List" view for display.
            return View(tags);
        }
        //READ Fuctionality
        // Create get method
        [HttpGet] // Specifies that this method handles HTTP GET requests.
        // Parameter has to match the name of the route created
        // Accepts a tag ID as a parameter from the URL.
        public IActionResult Edit(Guid id) 
        {
            // Use bloggieDbContext to connect to database to read details to display on screen and allow user to update or edit details
            //1st method var tag = bloggieDbContext.Tags.Find(id);
            //2nd method will find the tag using the id and the first one it finds it will give it back
            // Search for a tag in the database by its ID.
            // If a match is found, it returns the first occurrence; otherwise, it returns null.
            var tag = bloggieDbContext.Tags.FirstOrDefault(x => x.Id == id);

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
