Bloggie.Web is a ASP.NET CORE MVC USING .NET 8 BLOG WEBSITE.

I am building a .NET 8 ASP.NET CORE MVC web app Blog that will involve the following:

Overview of the Blog Application:

The blog application is designed to offer a platform where users can create, manage, and interact with blog posts. Key features typically include:​

User Authentication and Authorization: Implementing secure user registration and login functionalities using ASP.NET Core Identity.​

Post Management: Enabling users to perform CRUD operations on blog posts, including creating new posts, reading existing ones, updating content, and deleting posts when necessary.​

Commenting System: Allowing users to add comments to blog posts, fostering engagement and discussion.​

Tagging and Categorization: Organizing posts using tags and categories to enhance content discoverability.​

CRUD Operations in the Blog Application:

CRUD operations form the backbone of the blog application's functionality:​Medium

Create: Users can compose new blog posts by providing a title, content, and selecting relevant categories or tags. This operation involves rendering a form to the user and saving the input data to the database upon submission.​

Read: The application displays a list of all blog posts on the homepage, often with pagination for better navigation. Users can click on a post title to view the full content. This operation retrieves data from the database and presents it in a user-friendly format.​

Update: Authenticated users have the ability to edit their own posts. This involves fetching the existing post data, displaying it in an editable form, and saving the updated information back to the database.​

Delete: Users can remove their own posts when desired. This operation typically requires confirmation to prevent accidental deletions and involves removing the post record from the database.​

Implementation Details:

Models: Define classes representing the data structure of your application, such as Post, Comment, Category, and User. These models correspond to database tables and are used by Entity Framework Core to interact with the database.​

Controllers: Handle incoming HTTP requests, process user input, and return appropriate responses. For example, a PostsController would manage actions related to creating, reading, updating, and deleting blog posts.​

Views: Provide the user interface for the application. Views are responsible for rendering HTML pages that users interact with, such as forms for creating or editing posts and pages displaying lists of posts.

Model-View-Controller pattern
Entity Framework Core with code-first migrations and LINQ queries
ASP.NET Core Identity managing authentication and authorization with customizable user roles

I have used Visual Studio 2022 Community Edition to build this along with MS SQL / SQL Server Management Studio version 19

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


