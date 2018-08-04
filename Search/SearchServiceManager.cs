using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System.Configuration;
using System.Threading.Tasks;

namespace Search
{
    public class SearchServiceManager
    {
        SearchIndexClient _indexClient;
        SearchServiceClient _client;

        public async Task InitializeAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                var searchServiceName = ConfigurationManager.AppSettings["SearchService.Name"];
                var searchServiceKey = ConfigurationManager.AppSettings["SearchService.Key"];
                var credential = new SearchCredentials(searchServiceKey);

                _client = new SearchServiceClient(searchServiceName, credential);
                _indexClient = new SearchIndexClient(searchServiceName, "studentinfo", credential);
            });
        }

        public async Task CreateIndex()
        {
            if (!_client.Indexes.Exists("studentinfo"))
            {
                var index = new Index()
                {
                    Name = "studentinfo",
                    Fields = FieldBuilder.BuildForType<StudentInfo>()
                };
                await _client.Indexes.CreateAsync(index);
            }
        }

        public async Task UploadDocument(StudentInfo[] docs)
        {
            var batch = IndexBatch.Upload(docs);
            await _indexClient.Documents.IndexAsync(batch);
        }

        public async Task<DocumentSearchResult<StudentInfo>> Search(string searchString)
        {
            var param = new SearchParameters()
            {
                Select = new[] { "name", "stream" },
                SearchMode = SearchMode.Any,
            };
            return await _indexClient.Documents.SearchAsync<StudentInfo>(searchString, param);
        }
    }
}
