using Bookzilla.Database.Exceptions;
using Bookzilla.Database.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bookzilla.Models
{
    /// <summary>
    /// The Create/Edit Book view model.
    /// </summary>
    public class BookEditModel : IValidatableObject
    {
        /// <summary>
        /// The id of the book.
        /// </summary>
        [HiddenInput(DisplayValue = false)]
#pragma warning disable IDE1006 // Naming Styles
        public long? id { get; set; }
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// The AuthorId of the book (if selecting an existing author).
        /// </summary>
        [Display(Name = "Select an Author")]
        public long? AuthorId { get; set; }

        /// <summary>
        /// The author's name of the book (if inserting a new author).
        /// </summary>
        [Display(Name = "Or Enter a New Author")]
        public string AuthorName { get; set; }

        /// <summary>
        /// The title of the book.
        /// </summary>
        [Display(Name = "Title")]
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// A summary description of the book.
        /// </summary>
        [Display(Name = "Summary")]
        [DataType(DataType.MultilineText)]
        public string Summary { get; set; }

        /// <summary>
        /// The total number of pages in the book.
        /// </summary>
        [Display(Name = "Total Pages")]
        [Range(1, double.MaxValue, ErrorMessage = "Please enter a number greater than 0.")]
        public int? TotalPages { get; set; }

        /// <summary>
        /// The release date of the book.
        /// </summary>
        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; }

        /// <summary>
        /// The ISBN of the book.
        /// </summary>
        [Display(Name = "ISBN")]
        public string ISBN { get; set; }

        /// <summary>
        /// The number of books currently in stock.
        /// </summary>
        [Display(Name = "Quantity In Stock")]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a number greater than or equal to 0.")]
        public int? QuantityInStock { get; set; }

        /// <summary>
        /// Select list options for authors alread in the database.
        /// </summary>
        public List<SelectListItem> Authors { get; set; }

        /// <summary>
        /// Given the DbContext, loads the authors that are alread in the database.
        /// </summary>
        /// <param name="dbContext">The BookZillaEntities DbContext object.</param>
        public void InitializeLists(BookzillaEntities dbContext)
        {
            // check for null
            if (dbContext == null)
                throw new ArgumentNullException(nameof(dbContext));

            // Load the authors from the database
            this.Authors = dbContext.Authors.OrderBy(x => x.AUTHOR_DISPLAY_NAME).ToList()
                .Select(x => new SelectListItem() { Text = x.AUTHOR_DISPLAY_NAME, Value = x.AUTHOR_ID.ToString() }).ToList();

            // add the "select an author" list item
            this.Authors.Insert(0, new SelectListItem() { Text = "Please select an Author", Value = string.Empty });
        }

        /// <summary>
        /// Given the DbContext, loads the book from the database using the id specified.
        /// </summary>
        /// <param name="dbContext">The BookZillaEntities DbContext object.</param>
        public void Load(BookzillaEntities dbContext)
        {
            // check for null
            if (dbContext == null)
                throw new ArgumentNullException(nameof(dbContext));
            
            // try to locate the book in the database
            Book book = dbContext.Books.FirstOrDefault(x => x.BOOK_ID == this.id);
            if (book == null)
                throw new EntityNotFoundException(string.Format("Book was not found by id [{0}].", this.id));

            // set the model properties
            this.AuthorId = book.AUTHOR_ID;
            this.Title = book.BOOK_TITLE;
            this.Summary = book.BOOK_SUMMARY;
            this.TotalPages = book.BOOK_TOTAL_PAGES;
            this.ReleaseDate = book.BOOK_RELEASE_DATE;
            this.ISBN = book.BOOK_ISBN;
        }

        /// <summary>
        /// Given the DbContext, creates or updates the book in the database.
        /// </summary>
        /// <param name="dbContext">The BookZillaEntities DbContext object.</param>
        public void Save(BookzillaEntities dbContext)
        {
            // check for null
            if (dbContext == null)
                throw new ArgumentNullException(nameof(dbContext));

            // initialize the variable for the book
            Book book = null;

            // load the book from the database, if the id value is specified
            if (this.id.HasValue)
            {
                book = dbContext.Books.FirstOrDefault(x => x.BOOK_ID == this.id);

                // not found, do a 404
                if (book == null)
                    throw new EntityNotFoundException(string.Format("Book was not found by id [{0}].", this.id));
            }
            else // creating a new book
            {
                book = new Book();
            }

            // if the author's name is specified, we are creating a new author
            if (!string.IsNullOrEmpty(this.AuthorName))
            {
                // create the author object
                Author author = new Author() { AUTHOR_DISPLAY_NAME = this.AuthorName };

                // add and save
                dbContext.Authors.Add(author);
                dbContext.SaveChanges();

                // updat this object with the created author id value
                this.AuthorId = author.AUTHOR_ID;
            }

            // At this point we should have an author.
            if (!this.AuthorId.HasValue)
                throw new InvalidOperationException("AuthorId must have a value.");

            // set the book properties
            book.AUTHOR_ID = this.AuthorId.Value;
            book.BOOK_TITLE = this.Title;
            book.BOOK_SUMMARY = this.Summary;
            book.BOOK_TOTAL_PAGES = this.TotalPages;
            book.BOOK_RELEASE_DATE = this.ReleaseDate;
            book.BOOK_ISBN = this.ISBN;

            // if we are creating a new book, add to the DbSet
            if (!this.id.HasValue)
                dbContext.Books.Add(book);

            // saves/creates the book.
            dbContext.SaveChanges();

            // update the inventory in the database for the book
            dbContext.p_BookUpdateInventory(book.BOOK_ID, this.QuantityInStock);

            // update this object with the saved/created book
            this.id = book.BOOK_ID;
        }

        /// <summary>
        /// Additional validations to be performed on the view model for fitness for saving to the database.
        /// </summary>
        /// <param name="validationContext">A ValidationContext object.</param>
        /// <returns>An enumerable of any additional Validations for this BookEditModel object.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Must select an author or enter a new one
            if (!this.AuthorId.HasValue && string.IsNullOrEmpty(this.AuthorName))
                yield return new ValidationResult("Please select either an author or enter a new author.", new string[] { nameof(this.AuthorId) });

            // Cannot select an author and type one in, not sure which to use.
            if (this.AuthorId.HasValue && !string.IsNullOrEmpty(this.AuthorName))
                yield return new ValidationResult("Please select either an author or enter a new author.", new string[] { nameof(this.AuthorId) });
        }
    }
}