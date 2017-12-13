using System.Collections.Generic;
using BAU.Api.DAL.Models;

namespace BAU.Api.DAL.Repositories.Interface
{
    /// <summary>
    /// Enginner repository
    /// </summary>
    public interface IEnginnerRepository
    {
        /// <summary>
        /// Get a list of available Engineers
        /// </summary>
        /// <param name="count">Number of required engineers</param>
        /// <returns>List of engineers</returns>
        IList<Engineer> GetAvailableEngineers(int count); 
    }
}
