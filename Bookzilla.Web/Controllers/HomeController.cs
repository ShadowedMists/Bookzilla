using Bookzilla.Database.Models;
using Bookzilla.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bookzilla.Controllers
{
    /// <summary>
    /// The MVC Controller for the homepage
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// The homepage for the web application.
        /// </summary>
        /// <param name="model">A BookSearchModel object</param>
        /// <returns>A ViewResult object with a model of type BookSearchModel.</returns>
        public ActionResult Index(BookSearchModel model)
        {
            // Create a connection to the database, disposing of entities on completion
            using (BookzillaEntities dbContext = new BookzillaEntities())
            {
                // Load the books
                model.Load(dbContext);

                // return the view model
                return View(model);
            }
        }
    }
}