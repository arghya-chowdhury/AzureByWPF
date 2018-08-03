using Microsoft.WindowsAzure.Storage.Table;

namespace TableManagement
{
    public class StudentEntry:TableEntity
    {
        public string Name { get; set; }

        public string Role { get; set; }

        public string Stream { get; set; }
    }
}
