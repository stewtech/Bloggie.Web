<h1>Bloggie.Web is a .NET 8 ASP.NET Core MVC blog website that I am building, which will involve the following:</H1>

Overview of the Blog Application:

The blog application is designed to offer a platform where users can create, manage, and interact with blog posts. Key features typically include:​

User Authentication and Authorization: Implementing secure user registration and login functionalities using ASP.NET Core Identity.​

Post Management: Enabling users to perform CRUD operations on blog posts, including creating new posts, reading existing ones, updating content, and deleting posts when necessary.​

Commenting System: Allowing users to add comments to blog posts, fostering engagement and discussion.​

Tagging and Categorization: Organizing posts using tags and categories to enhance content discoverability.​

CRUD Operations in the Blog Application:

CRUD operations form the backbone of the blog application's functionality

Create: Users can compose new blog posts by providing a title, content, and selecting relevant categories or tags. This operation involves rendering a form to the user and saving the input data to the database upon submission.​

Read: The application displays a list of all blog posts on the homepage, often with pagination for better navigation. Users can click on a post title to view the full content. This operation retrieves data from the database and presents it in a user-friendly format.​

Update: Authenticated users have the ability to edit their own posts. This involves fetching the existing post data, displaying it in an editable form, and saving the updated information back to the database.​

Delete: Users can remove their own posts when desired. This operation typically requires confirmation to prevent accidental deletions and involves removing the post record from the database.​

Implementation Details:

Models: I define classes that represent the data structure of my application, such as Post, Comment, Category, and User. These models correspond to database tables and allow me to use Entity Framework Core to interact with the database.

Controllers: Handle incoming HTTP requests, process user input, and return appropriate responses. For example, a PostsController would manage actions related to creating, reading, updating, and deleting blog posts.​

Views: Provide the user interface for the application. Views are responsible for rendering HTML pages that users interact with, such as forms for creating or editing posts and pages displaying lists of posts.



I have used Visual Studio 2022 Community Edition to build this along with MS SQL / SQL Server Management Studio version 19

<h1>Code Walk Through</h1>

I created domain models called Blogpost.cs and Tag.cs

I installed the Nuget Packages called Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools

I created a DbContext Class to interact with the SQL Database and to perform CRUD operations

I then added a connection string inside appsettings.json

I then add Dependency Injection into Program.cs 

Next I did the EF Core Migrations

Add-Migration "Initial Migration"

Update-Database

Next, I made the navbar dark using Bootstrap

Then, I removed the privacy link and turned it into a dropdown nav item with Bootstrap and added the dropdown item called Add Tag

Then, I created an new controller called AdminTags

using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
    }
}

I then changed the action to Add(), and made it a getter by adding [HttpGet]

This created a new view called AdminTags

Within this view I have created a form with the input fields Name and Display Name and a Submit button.

Then, I added a [HttpPost] method to the controller because when you submit the form it goes to the post method

Then, I created a ViewModels folder and within that ViewModels folder I added a new class called AddTagRequest.cs

Within the class called AddTagRequest, I added the properties

namespace Bloggie.Web.Models.ViewModels
{
    public class AddTagRequest
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}


I then added Model Binding to the from by adding @model Bloggie.Web.Models.ViewModels.AddTagRequest

Then I bound the Model to the View by adding asp-for attributes within the form elements

@model Bloggie.Web.Models.ViewModels.AddTagRequest
@{

}


<div class="bg-secondary bg-opacity-10 py-2">
    <div class="container">
        <h1>Add New Tag - Admin Functionality</h1>
    </div>
</div>

<div class="container py-5">
    <form method="post">
        <div class="mb-3">
            <label class="form-label">Name</label>
            <input type="text" class="form-control" id="name" asp-for="Name"/>   
        </div>

        <div class="mb-3">
            <label class="form-label">Display Name</label>
            <input type="text" class="form-control" id="displayName" asp-for="DisplayName" />
        </div>

        <div class="mb-3">   
            <button type="submit" class="btn btn-dark">Submit</button>
        </div>
    </form>
</div>

Now, the Model will be coming as a request so I needed to put it in as the parameter for the IActionResult HttpPost method in the controller

using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public IActionResult Add(AddTagRequest addTagRequest)
        {
            var name = addTagRequest.Name;
            var displayName = addTagRequest.DisplayName;

            return View("Add");
        }
    }
}

The properties within the AddTagRequest Model then needed to be added within the IActionResult HttpPost method as variables

Once I have seen by WATCHING the values coming back from the controller, I then removed the variables within the method

[HttpPost]
[ActionName("Add")]
public IActionResult Add(AddTagRequest addTagRequest)
{
   

    return View("Add");
}

Now, I needed to make use of the DbContext to save the tag to the Database.

Using constructor injection from the Program.cs file, so I created a constructor for this class

using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
   
    public class AdminTagsController : Controller
    {
        public AdminTagsController()
        {
            
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public IActionResult Add(AddTagRequest addTagRequest)
        {
           

            return View("Add");
        }
    }
}


Then added BloggieDbContext as a parameter

I then made a private variable to be used in the method

Then I initialized it within the public AdminTagsController method

public class AdminTagsController : Controller
{
    private BloggieDbContext _bloggieDbContext;
    public AdminTagsController(BloggieDbContext bloggieDbContext)
    {
        _bloggieDbContext = bloggieDbContext;
    }

After I did that I removed the private variable and by clicking on the bloggieDbContext parameter and pressing control . 
I then had a popup to choose from and chose the 
Create and assign field bloggieDbContext and Visual Studio automatically entered a private variable and initialized it within the method like this:

     public class AdminTagsController : Controller
 {
     private readonly BloggieDbContext bloggieDbContext;

     public AdminTagsController(BloggieDbContext bloggieDbContext)
     {
         this.bloggieDbContext = bloggieDbContext;


After injecting bloggieDbContext I want to use it to save the Tag to the database

Within the HttpPost IActionResult method, I used the private variable 
        
bloggieDbContext.Tags.Add(tag);

This line of code will access the Tags from the DbContext file and will access the DbSet property named Tags

Now I need to create the Add request method into a domain.Tag model

So I created a new variable called tag

Then used the domain model called Tag which is coming form bloggie.web.models.domain

Then a using statement was added

using Bloggie.Web.Models.Domain;

Then using open and closing brackets assign values from addTagRequest inside Tag

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
    return View("Add");
}

So I mapped the AddTagRequest to Tag domain Model

Then I supplied the tag variable into the Add() method

<h2>(Create Functionality)</h2>

Now bloggieDbContext will be able to create the tag inside the Tags table in the database 

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

Then I added the SaveChanges() to save changes to the database

bloggieDbContext.SaveChanges();

Next, I want to be able to read all the records from the tag table and show them in a list in a Bootstrap table

Within the AdminTagsController.cs file, this files will be responsible for all the CRUD operations

I created a page where I can display the list of tags coming from the database (will read from database and show a list)

<h2>(Read Functionality)</h2>

I also made sure the Show All Tags dropdown in the the navigation menu will show tags in a list

<li><a class="dropdown-item" asp-area="" asp-controller="AdminTags" asp-action="List">Show All Tags</a></li>

 // Create new page where I can display the list of tags coming from the database 
 [HttpGet]
 [ActionName("List")]
 public IActionResult List()
 {
     // use dbcontext to read the tags
     var tags = bloggieDbContext.Tags.ToList();

     return View(tags);
 }

 <h2>(Update Functionality)</h2>

Then I added a HttpGet IActionResult method into the AdminTagsController

  // Create get method
  [HttpGet]
  // Parameter has to match the name of the route created
  public IActionResult Edit(Guid id) 
  { 
      return View();
  }

I made sure when I edit, the Id will be chosen by adding the asp-route-id attribute

 <td>
     <a asp-area="" asp-controller="AdminTags" asp-action="Edit" asp-route-id="@tag.Id">Edit</a>
 </td>

 Then within the Edit method in the AdminTagsController.cs file, I added a variable to get back the first id

   public IActionResult Edit(Guid id) 
  { 
      // Use bloggieDbContext to connect to database to read details to display on screen and allow user to update or edit details
      //1st method var tag = bloggieDbContext.Tags.Find(id);
      //2nd method will find the tag using the id and the first one it finds it will give it back
      var tag = bloggieDbContext.Tags.FirstOrDefault(x => x.Id == id);

      // display tag into edit page


      return View();
  }

  Next, I create a new model to go with the Edit.cshtml view. I will make a new model for each view.

  <h2>Edit Functionality</h2>

  namespace Bloggie.Web.Models.ViewModels
{
    public class EditTagRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }
    }
}

View functionality within the AdminTagsController.cs

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

Then I made a form with model binding in the edit view

<div class="container py-5">
    <form method="post">

        <div class="mb-3">  
            <label class="form-label">Id</label>
            <input type="text" id="id" class="form-control" asp-for="Id" readonly/>
        </div>
        <div class="mb-3">
            <label class="form-label">Name</label>
            <input type="text" id="Name" class="form-control" asp-for="Name" />
        </div>
        <div class="mb-3">
            <label class="form-label">Display Name</label>
            <input type="text" id="displayName" class="form-control" asp-for="DisplayName" />
        </div>

        <div class="mb-3">
            <button type="submit" class="btn btn-dark">Update</button>
        </div>


    </form>
</div>

Next, I created a post method in the AdminTagsController.cs file

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
             return RedirectToAction("Edit", new { id = editTagRequest.Id });
            
         }
         /// If the tag does not exist, still redirect back to the Edit page.
         return RedirectToAction("Edit", new { id = editTagRequest.Id });  
     }

This will handle form submissions for editing a tag in a blogging application.

Receives Form Data:

It takes an EditTagRequest object as a parameter, which likely contains properties like Id, Name, and DisplayName.

Creates a New Tag Object:

A Tag object is instantiated using values from editTagRequest.

Finds the Existing Tag in the Database:

It queries the bloggieDbContext.Tags table to find a tag with the given Id.

Updates the Existing Tag:

If a matching tag is found, its Name and DisplayName are updated.

Saves Changes to Database:

bloggieDbContext.SaveChanges(); commits the updates to the database.

Redirects to the Edit Page:

Regardless of success or failure, the method redirects back to the Edit page for the given tag ID.

So far the AdminTagsController.cs file looks like this:

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
        // READ FUNCTIONALITY
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
        // UPDATE FUNCTIONALIY
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

        // DELETE FUNCTIONALITY
        // Specifies that this action method will handle POST requests
        [HttpPost]
        // Defines the Delete action method, accepting an EditTagRequest object as a parameter
        public IActionResult Delete(EditTagRequest editTagRequest) 
        {
            // Searches for the tag in the database using the provided ID from the editTagRequest
            var tag = bloggieDbContext.Tags.Find(editTagRequest.Id);
            // Checks if the tag was found in the database
            if (tag != null)
            {
                // Removes the found tag from the context (i.e., marks it for deletion)
                bloggieDbContext.Tags.Remove(tag);
                // Saves the changes to the database (actually deletes the tag)
                bloggieDbContext.SaveChanges();

                // Show a success notification
                // Redirects the user to the "List" action, likely to show the updated list of tags
                return RedirectToAction("List");
            }
            // Show and error notification
            // If the tag wasn't found, redirects to the "Edit" action with the same ID, indicating an error occurred
            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }

    }
}

I then made a Delete Button in the Edit.cshtml view file and added some razor code. 

Next, I will add async functionality to the methods in the AdminTagsController.cs file. 

I also wrapped my form in Edit.cshtml inside a If/Else statment to check if the Model is null or not.

Prevents Null Reference Exceptions
If the Model is null and your view attempts to access properties of the Model, it can cause a runtime exception. Using an if statement ensures that the form is only rendered when the Model contains data.

Improves Code Maintainability
Keeping explicit control over when a form should be displayed makes it easier for other developers (or your future self) to understand the intended behavior of the page.

Prevents Unintended Submissions
If a form is rendered with a null Model, the user might submit it with incomplete or invalid data. By conditionally displaying the form, you can ensure that users only interact with the form when it's meaningful.

Better Error Handling
Instead of letting the application fail due to null values, you can display a helpful error message or redirect users to an appropriate page.

This approach ensures that the form is only shown when the Model is available, avoiding potential errors and improving the user experience.

This should be done when:

When the Model Is Expected to Be Required for the Form
If the form is meant to edit an existing entity (like updating a blog post), but the Model is null, the form should not be shown.

Example: Editing a blog post that doesn’t exist in the database.

When Rendering Model Data Inside the Form
If your form fields are pre-filled with Model properties, a null Model could cause an exception.

When Preventing Submissions with Invalid or Missing Data
If the form depends on required data, hiding it prevents users from submitting incomplete forms.

Example: Preventing a user from submitting a form if the necessary data is not loaded.

When the Model Is Fetched Dynamically
If the Model is retrieved from a database or API call and might not always be available.

Example: A user tries to access an edit page for a blog post that has been deleted.

When Handling Edge Cases
If certain conditions cause the Model to be null, such as incorrect URLs, missing parameters, or user permissions.

How to Know When NOT to Do This?
If your form is for creating a new entry (like a new blog post), checking for null isn’t necessary since the form fields will be empty anyway.

If the form doesn’t depend on the Model at all (e.g., a basic contact form), you don’t need to check.

 @if (Model != null)
 {
     <form method="post">

         <div class="mb-3">
             <label class="form-label">Id</label>
             <input type="text" id="id" class="form-control" asp-for="Id" readonly />
         </div>
         <div class="mb-3">
             <label class="form-label">Name</label>
             <input type="text" id="Name" class="form-control" asp-for="Name" />
         </div>
         <div class="mb-3">
             <label class="form-label">Display Name</label>
             <input type="text" id="displayName" class="form-control" asp-for="DisplayName" />
         </div>

         <div class="mb-3">
             <div class="d-flex">
                 <button type="submit" class="btn btn-dark">Update</button>
                 <!-- Button for deleting, with danger styling (red button) and additional margin to the left (ms-2) -->
                 <!--asp-area="": This specifies the area in the application where the controller is located. In this case, it’s left empty, meaning the controller is in the default area.

                 asp-controller="AdminTags": This specifies the controller to be used when the button is clicked, which is AdminTags in this case.

                 asp-action="Delete": This specifies the action method to be invoked in the AdminTags controller when the button is clicked. The action method is Delete.-->
                 <button class="btn btn-danger ms-2" type="submit" asp-area="" asp-controller="AdminTags" asp-action="Delete">Delete</button>
             </div>
         </div>
     </form>
 }
 else
 {
     <p>Tag Not Found!</p>
 }

Next, I will add the Repsitory Pattern

The Repository Pattern in an ASP.NET Core MVC .NET 8 blog project improves maintainability, testability, and flexibility by decoupling the data access layer from the business logic. It also enhances code reuse and makes it easier to manage changes in the future.

Why should the Repository Pattern be used?

1. Separation of Concerns
Keeps the data access logic separate from the business logic in controllers and services.

Makes the application more organized and maintainable.

2. Encapsulation of Data Access Logic
Centralizes data queries and CRUD operations in one place.

If you need to change your database provider (e.g., switching from SQL Server to PostgreSQL), you only need to modify the repository layer.

3. Improved Testability
By abstracting data access, you can mock repositories in unit tests instead of relying on an actual database.

Makes testing easier and faster compared to integration tests that hit a real database.

4. Decoupling from EF Core
If you directly use Entity Framework Core (EF Core) in controllers, your business logic becomes tightly coupled to it.

Using a repository pattern reduces this dependency, allowing easier migration or changes in ORM frameworks.

5. Better Code Reusability
Common database operations (e.g., GetAll(), GetById(id), Add(entity), Update(entity), Delete(id)) can be shared across multiple parts of the application.

Avoids code duplication in different controllers or services.

6. Abstraction for Business Logic
You can introduce an interface (IRepository<T>), making it easy to swap out implementations (e.g., switching from EF Core to Dapper or another data source).

7. Simplified Data Caching and Logging
If you need caching, logging, or performance optimizations, you can implement them inside the repository layer instead of modifying multiple controllers.

I then created a Interface in a new folder called Repositories.

using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
    public interface ITagRepository
    {
        // Create definitions/Methods for CRUD operations
        Task<IEnumerable<Tag>> GetAllAsync();

        Task<Tag?> GetAsync(Guid id);

        Task<Tag> AddAsync(Tag tag);

        Task<Tag?> UpdateAsync(Tag tag);

        Task<Tag?> DeleteAsync(Guid id);

    }
}

Then I created a class inside the Repositories folder called TagRepository.cs

Then I inherited from the ITagRepository, all the methods within it. 

using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
    public class TagRepository : ITagRepository
    {
    
        public Task<Tag> AddAsync(Tag tag)
        {
            throw new NotImplementedException();
        }

        public Task<Tag?> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Tag>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Tag?> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Tag?> UpdateAsync(Tag tag)
        {
            throw new NotImplementedException();
        }
    }
}

Next, I created a class called Repository within the Repositories folder.

It inherites from the ITagRepository.

I made a constructor that has a parameter of bloggieDbContext

  private readonly BloggieDbContext bloggieDbContext;

  public TagRepository(BloggieDbContext bloggieDbContext)
  {
      this.bloggieDbContext = bloggieDbContext;
  }

Within the first method in the TagRepository file I moved the code that accesses the database from the Add method in the AdminTagsController.cs file. 

  public async Task<Tag> AddAsync(Tag tag)
  {
      // Adds the new tag to the database.
      await bloggieDbContext.Tags.AddAsync(tag);
      // Saves changes to the database to persist the new tag.
      await bloggieDbContext.SaveChangesAsync();
      return tag;
  }

  



