using Bookzilla.Database.Exceptions;
using Bookzilla.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bookzilla.Models
{
    /// <summary>
    /// The Delete Book view model.
    /// </summary>
    public class BookDeleteModel
    {
        /// <summary>
        /// The id of the book.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public long? id { get; set; }
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// The Title of the book.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The Author of the book.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Given the DbContext, deletes the book with the id specified.
        /// </summary>
        /// <param name="dbContext">The BookZillaEntities DbContext object.</param>
        public void Load(BookzillaEntities dbContext)
        {
            // check for null
            if (dbContext == null)
                throw new ArgumentNullException(nameof(dbContext));

            // load the book
            v_BookSummary book = dbContext.v_BookSummary.FirstOrDefault(x => x.BOOK_ID == this.id);
            if(book == null)
                throw new EntityNotFoundException(string.Format("Book was not found by id [{0}].", this.id));

            // set the model properties
            this.Title = book.BOOK_TITLE;
            this.Author = book.AUTHOR_DISPLAY_NAME;
        }
    }
}