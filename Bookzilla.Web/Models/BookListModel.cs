using Bookzilla.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bookzilla.Models
{
    /// <summary>
    /// View model for the Book Index page.
    /// </summary>
    public class BookListModel
    {
        /// <summary>
        /// A list of v_BookSummary objects.
        /// </summary>
        public List<v_BookSummary> Books { get; protected set; }

        /// <summary>
        /// Given the DbContext, loads the books in the database.
        /// </summary>
        /// <param name="dbContext">The BookZillaEntities DbContext object.</param>
        public void Load(BookzillaEntities dbContext)
        {
            // check for null
            if (dbContext == null)
                throw new ArgumentNullException(nameof(dbContext));

            // load all the books
            this.Books = dbContext.v_BookSummary.OrderBy(x => x.BOOK_TITLE).ToList();
        }
    }
}