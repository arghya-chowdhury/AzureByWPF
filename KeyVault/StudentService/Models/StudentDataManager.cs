using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Azure.KeyVault;
using System.Web.Configuration;
using System;

namespace StudentDataService
{
    public class StudentDataManager
    {
        CloudTableClient client;

        public void Initialize()
        {
            var connectionString = GetSecret();
            var account = CloudStorageAccount.Parse(connectionString);
            client = account.CreateCloudTableClient();
        }

        public static string GetSecret()
        {
            var kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetAccessToken));
            var secret = kv.GetSecretAsync(WebConfigurationManager.AppSettings["SecretIdentifier"]).GetAwaiter().GetResult();
            return secret?.Value;
        }

        public static async Task<string> GetAccessToken(string authority, string resource, string scope)
        {
            var context = new AuthenticationContext(authority);
            ClientCredential clientCredential = new ClientCredential(WebConfigurationManager.AppSettings["ClientId"], WebConfigurationManager.AppSettings["ClientSecret"]);
            var tokenResponse = await context.AcquireTokenAsync(resource, clientCredential);
            var accessToken = tokenResponse.AccessToken;
            return accessToken;
        }

        public IList<StudentEntry> GetStudents()
        {
            var table = client.GetTableReference("student");
            //If Exists Throws Exception, Hence Check Is Required
            if (!table.Exists())
            {
                table.CreateIfNotExists();
            }

            var query = new TableQuery<StudentEntry>();
            var data = table.ExecuteQuery(query);
            return data.Any() ? data.ToList() : new List<StudentEntry>();
        }

        public StudentEntry GetStudent(string rowKey)
        {
            var table = client.GetTableReference("student");
            //If Exists Throws Exception, Hence Check Is Required
            if (!table.Exists())
            {
                table.CreateIfNotExists();
            }

            var query = new TableQuery<StudentEntry>().Where(
                TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "student"),
                TableOperators.And,
                TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey)));
            var data = table.ExecuteQuery(query);
            return data.FirstOrDefault();
        }

        public async Task<TableResult> AddStudent(StudentEntry student)
        {
            try
            {
                student.PartitionKey = "student";
                student.RowKey = student.Role;

                var table = client.GetTableReference("student");

                //If Exists Throws Exception, Hence Check Is Required
                if (!table.Exists())
                {
                    table.CreateIfNotExists();
                }

                var tableOperation = TableOperation.Insert(student);
                return await table.ExecuteAsync(tableOperation);
            }
            catch
            {
                return await Task.FromResult(new TableResult() { HttpStatusCode = (int)HttpStatusCode.InternalServerError });
            }
        }

        public async Task<TableResult> DeleteStudent(StudentEntry student)
        {
            try
            {
                var table = client.GetTableReference("student");
                //If Exists Throws Exception, Hence Check Is Required
                if (!table.Exists())
                {
                    table.CreateIfNotExists();
                }

                var tableOperation = TableOperation.Delete(student);
                return await table.ExecuteAsync(tableOperation);
            }
            catch
            {
                return await Task.FromResult(new TableResult() { HttpStatusCode = (int)HttpStatusCode.InternalServerError });
            }
        }
    }
}
