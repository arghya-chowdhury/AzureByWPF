using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace StorageManagement
{
    public enum CrudOperation
    {
        Create,
        Update,
        Delete,
    }

    public class TableManager
    {
        CloudTableClient client;

        public async Task InitializeAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                var connectionString = ConfigurationManager.ConnectionStrings["QuickStartStorage"].ConnectionString;
                var account = CloudStorageAccount.Parse(connectionString);
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
