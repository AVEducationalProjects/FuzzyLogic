using FuzzyLogic.Portal.Model;
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
    public class RuleMongoRepositary : IRuleRepositary
    {
        public IMongoCollection<FuzzyRule> Rules { get; set; }

        public RuleMongoRepositary(IOptions<MongoOptions> mongoOptions)
        {
            var client = new MongoClient(mongoOptions.Value.Server);
            var database = client.GetDatabase(mongoOptions.Value.Database);

            Rules = database.GetCollection<FuzzyRule>(mongoOptions.Value.RulesCollection);
        }

        public async Task Delete(ObjectId id)
        {
            await Rules.FindOneAndDeleteAsync(x => x.Id == id.ToString());
        }

        public async Task<FuzzyRule> Get(ObjectId id)
        {
            var result = await Rules.FindAsync(x => x.Id == id.ToString());
            return result.Single();
        }

        public async Task<IList<FuzzyRule>> List()
        {
            var result = await Rules.FindAsync(_ => true);
            return result.ToList();
        }

        public async Task Save(FuzzyRule rule)
        {
            var result = await Rules.FindAsync(x => x.Id == rule.Id.ToString());
            if (result.Any())
            {
                await Rules.FindOneAndReplaceAsync(x=>x.Id ==rule.Id, rule);
            }
            else
            {
                await Rules.InsertOneAsync(rule);
            }
        }
    }
}
