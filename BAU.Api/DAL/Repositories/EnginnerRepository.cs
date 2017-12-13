using System.Collections.Generic;
using BAU.Api.DAL.Models;
using BAU.Api.DAL.Repositories.Interface;

namespace BAU.Api.DAL.Repositories
{
    public class EnginnerRepository : IEnginnerRepository
    {
        public IList<Engineer> GetAvailableEngineers(int count)
        {
            throw new System.NotImplementedException();
        }
    }
}
