using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navislamia.Game.DataAccess.Repositories.Interfaces
{
    public interface ISummonRepository
    {

        /// <summary>
        /// Avoid using SaveChanges directly from context as it applies modifications directly to the database.
        /// Finish all required operations for a step then call this method
        /// </summary>
        Task SaveChangesAsync();
    }
}
