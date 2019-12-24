using FuzzyLogic.Portal.Model;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FuzzyLogic.Portal.Data
{
    public interface IRuleRepositary
    {
        Task<IList<FuzzyRule>> List();

        Task<FuzzyRule> Get(ObjectId id);

        Task Save(FuzzyRule rule);

        Task Delete(ObjectId id);
    }
}
