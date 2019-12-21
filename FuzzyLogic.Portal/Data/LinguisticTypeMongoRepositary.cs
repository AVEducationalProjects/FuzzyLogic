using FuzzyLogic.Portal.Options;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuzzyLogic.Portal.Data
{
    public class LinguisticTypeMongoRepositary : ILinguisticTypeRepositary
    {
        public IMongoCollection<LinguisticType> LinguisticTypes { get; set; }

        public LinguisticTypeMongoRepositary(IOptions<MongoOptions> mongoOptions)
        {
            var client = new MongoClient(mongoOptions.Value.Server);
            var database = client.GetDatabase(mongoOptions.Value.Database);

            LinguisticTypes = database.GetCollection<LinguisticType>(mongoOptions.Value.LinguisticTypeCollection);
        }

        public async Task Delete(ObjectId id)
        {
            await LinguisticTypes.FindOneAndDeleteAsync(x => x.Id == id.ToString());
        }

        public async Task<LinguisticType> Get(ObjectId id)
        {
            var result = await LinguisticTypes.FindAsync(x => x.Id == id.ToString());
            return result.Single();
        }

        public async Task<IList<LinguisticType>> List()
        {
            var result = await LinguisticTypes.FindAsync(_ => true);
            return result.ToList();
        }

        public async Task Save(LinguisticType linguisticType)
        {
            var existed = await LinguisticTypes.FindAsync(x => x.Id == linguisticType.Id);
            if (existed.Any())
            {
                await LinguisticTypes.FindOneAndReplaceAsync(x => x.Id == linguisticType.Id, linguisticType);
            }
            else
            {
                await LinguisticTypes.InsertOneAsync(linguisticType);
            }
        }
    }
}
