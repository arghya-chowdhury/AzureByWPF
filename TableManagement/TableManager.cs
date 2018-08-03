using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TableManagement
{
    public enum CrudOperation
    {
        Create,
        Update,
        Delete,
    }

    public class TableManager
    {
        //ConnectionString Contains AccessKey for Public Blog
        const string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=studentgeneralstorage;AccountKey=bsaoityqDOY/2q6NLaQYuHYGlpbK+8RxZ4Tnzpzt1xBY65VzSFpLkjP+Ezie1gTQ+5u4TVVKcY2fFVmk/arzyA==;EndpointSuffix=core.windows.net";
        CloudTableClient client;

        public async Task InitializeAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                CloudStorageAccount account = CloudStorageAccount.Parse(ConnectionString);
                client = account.CreateCloudTableClient();
            });
        }

        public async Task<TableResult> PerformOperationTableAsync(CrudOperation op, StudentEntry studentEntry = null)
        {
            if (studentEntry == null)
            {
                return null;
            }

            studentEntry.PartitionKey = "student";
            studentEntry.RowKey = studentEntry.Role;

            var table = client.GetTableReference("student");

            //If Exists Throws Exception, Hence Check Is Required
            if (!table.Exists())
            {
                table.CreateIfNotExists();
            }

            TableOperation tableOp = null;

            switch (op)
            {
                case CrudOperation.Create:
                    tableOp = TableOperation.Insert(studentEntry);
                    return await table.ExecuteAsync(tableOp);

                case CrudOperation.Delete:
                    tableOp = TableOperation.Delete(studentEntry);
                    return await table.ExecuteAsync(tableOp);

                case CrudOperation.Update:
                    tableOp = TableOperation.Replace(studentEntry);
                    return await table.ExecuteAsync(tableOp);
            }
            return null;
        }

        public IList<StudentEntry> GetTableData()
        {
            var table = client.GetTableReference("student");
            //If Exists Throws Exception, Hence Check Is Required
            if (!table.Exists())
            {
                table.CreateIfNotExists();
            }

            TableQuery<StudentEntry> query = new TableQuery<StudentEntry>().Where(
            TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "student"));
            var data = table.ExecuteQuery(query);
            return data.Any() ? data.ToList() : new List<StudentEntry>();
        }
    }
}
