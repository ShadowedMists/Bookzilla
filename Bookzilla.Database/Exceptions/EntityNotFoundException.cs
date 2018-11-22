using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookzilla.Database.Exceptions
{
    /// <summary>
    /// A throwable exception to indicate that a database entity was expected but could not be located.
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException() : base() { }

        public EntityNotFoundException(string message) : base(message) { }
    }
}
