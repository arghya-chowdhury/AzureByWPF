using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosDB
{
    class CosmosDBManager
    {
        DocumentClient _client;

        string _databaseId;
        string _collectionId;

        public CosmosDBManager()
        {
            var uri = ConfigurationManager.AppSettings["Microsoft.CosmosDB.Endpoint"];
            var key = ConfigurationManager.AppSettings["Microsoft.CosmosDB.Key"];
            _client = new DocumentClient(new Uri(uri), key);

            _databaseId = ConfigurationManager.AppSettings["Microsoft.CosmosDB.Database.Id"];
            _collectionId = ConfigurationManager.AppSettings["Microsoft.CosmosDB.Database.Collection.Id"];

        }

        public async Task CreateDocuments(TaskItem[] items)
        {
            await Task.Factory.StartNew(() =>
            {
                var tasks = new List<Task>();
                items.ToList().ForEach(item =>
                {
                    var docColUri = UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId);
                    tasks.Add(_client.CreateDocumentAsync(docColUri, item));
                });
                Task.WaitAll(tasks.ToArray());
            });
        }

        public IList<TaskItem> RetrieveDocuments()
        {
            var docColUri = UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId);
            return _client.CreateDocumentQuery<TaskItem>(docColUri, new FeedOptions() { MaxItemCount = 100 })
                                            .ToList();
        }

        public async Task DeleteDocuments(string id)
        {
            var documentLink = UriFactory.CreateDocumentUri(_databaseId, _collectionId, id);
            await _client.DeleteDocumentAsync(documentLink);
        }
    }
}
