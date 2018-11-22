using Bookzilla.Database.Exceptions;
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
    /// An MVC Controller that manages the Books that are saved in the Bookzilla system.
    /// </summary>
    public class BookController : Controller
    {
        /// <summary>
        /// The Entity Framework DbContext that communicates with the Bookzilla database.
        /// </summary>
        protected BookzillaEntities dbContext = new BookzillaEntities();

        /// <summary>
        /// Destructor for the BookController to dispose of the database context.
        /// </summary>
        ~BookController()
        {
            this.dbContext?.Dispose();
            this.dbContext = null;
        }

        /// <summary>
        /// The Book list page.
        /// </summary>
        /// <returns>An ViewResult object with a model of type BookListModel.</returns>
        public ActionResult Index()
        {
            // initialize the view model
            BookListModel model = new BookListModel();
            model.Load(dbContext);

            // Show the list page
            return View(model);
        }

        /// <summary>
        /// The Create Book Page for loading.
        /// </summary>
        /// <returns>An ViewResult object with a model of type BookEditModel.</returns>
        [HttpGet, ActionName("Create")]
        public ActionResult CreateGet()
        {
            // initialize the view model
            BookEditModel model = new BookEditModel();
            model.InitializeLists(this.dbContext);

            // Show the create page
            return View("Edit", model);
        }

        /// <summary>
        /// The Create Book Page for saving.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>A RedirectToRouteResult object with an action of Index.</returns>
        [HttpPost, ActionName("Create"), ValidateAntiForgeryToken]
        public ActionResult CreatePost(BookEditModel model)
        {
            try
            {
                // check if all the validations passed.
                if (!this.ModelState.IsValid)
                {
                    // load the author list and return the view model
                    model.InitializeLists(this.dbContext);
                    return View("Edit", model);
                }

                // call save on the view model
                model.Save(this.dbContext);

                // redirect back to the book list page
                return RedirectToAction("Index");
            }
            catch (EntityNotFoundException) // if the requested author is not found, do a 404 
            {
                return this.HttpNotFound();
            }
        }

        /// <summary>
        /// The Edit Book Page for loading.
        /// </summary>
        /// <param name="id">The id of the Book to edit.</param>
        /// <returns>An ViewResult object with a model of type BookEditModel.</returns>
        [HttpGet, ActionName("Edit")]
        public ActionResult EditGet(long? id)
        {
            try
            {
                // initialize the view model
                BookEditModel model = new BookEditModel() { id = id };
                model.InitializeLists(this.dbContext);
                model.Load(this.dbContext);

                // show the edit page
                return View(model);
            }
            catch (EntityNotFoundException) // if the book was not located, do a 404
            {
                return this.HttpNotFound();
            }
        }

        /// <summary>
        /// The Edit Book Page for saving.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>A RedirectToRouteResult object with an action of Index.</returns>
        [HttpPost, ActionName("Edit"), ValidateAntiForgeryToken]
        public ActionResult EditPost(BookEditModel model)
        {
            try
            {

                // check if all the validations passed.
                if (!this.ModelState.IsValid)
                {
                    // load the author list and return the view model
                    model.InitializeLists(this.dbContext);
                    return View(model);
                }

                // call save on the view model
                model.Save(this.dbContext);

                // redirect back to the book list page
                return RedirectToAction("Index");
            }
            catch (EntityNotFoundException) // if the book was not located or the author, do a 404
            {
                return this.HttpNotFound();
            }
        }

        /// <summary>
        /// The Delete Book Page for loading.
        /// </summary>
        /// <param name="id">The id of the Book to delete.</param>
        /// <returns>An ViewResult object with a model of type BookDeleteModel.</returns>
        [HttpGet, ActionName("Delete")]
        public ActionResult DeleteGet(long? id)
        {
            try
            {
                // initialize the view model
                BookDeleteModel model = new BookDeleteModel() { id = id };
                model.Load(this.dbContext);

                // show the delete page
                return View(model);
            }
            catch (EntityNotFoundException)
            {
                return this.HttpNotFound();
            }
        }

        /// <summary>
        /// The Delete Book Page for deletion.
        /// </summary>
        /// <param name="id">The id of the Book to delete.</param>
        /// <returns>A RedirectToRouteResult object with an action of Index.</returns>
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public ActionResult DeletePost(long? id)
        {
            // identify the book in question
            Book b = this.dbContext.Books.FirstOrDefault(x => x.BOOK_ID == id);

            // if it cannot be located, 404
            if (b == null)
                return this.HttpNotFound();

            // delete the book
            this.dbContext.Books.Remove(b);
            this.dbContext.SaveChanges();

            // redirect back to the book list page
            return RedirectToAction("Index");
        }
    }
}