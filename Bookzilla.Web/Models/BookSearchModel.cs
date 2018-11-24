using Bookzilla.Database.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bookzilla.Models
{
    /// <summary>
    /// View Model for the Homepage. Has the ability to search the books in the database.
    /// </summary>
    public class BookSearchModel
    {
        /// <summary>
        /// The search text to locate books in the database.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public string q { get; set; }
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// A list of p_BookSearch_Result objects identified using the search terms.
        /// </summary>
        public List<p_BookSearch_Result> Books { get; set; }

        /// <summary>
        /// Given the DbContext, loads the books in the database using the search terms specified.
        /// </summary>
        /// <param name="dbContext">The BookZillaEntities DbContext object.</param>
        public void Load(BookzillaEntities dbContext)
        {
            // check for null
            if (dbContext == null)
                throw new ArgumentNullException(nameof(dbContext));
            
            // execute the search
            this.Books = dbContext.p_BookSearch(q).ToList();
        }
    }
}