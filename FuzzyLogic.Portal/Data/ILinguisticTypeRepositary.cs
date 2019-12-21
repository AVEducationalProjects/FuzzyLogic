using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FuzzyLogic.Portal.Data
{
    public interface ILinguisticTypeRepositary
    {
        Task<IList<LinguisticType>> List();
        Task Save(LinguisticType linguisticType);
        Task<LinguisticType> Get(ObjectId id);
        Task Delete(ObjectId id);
    }
}
