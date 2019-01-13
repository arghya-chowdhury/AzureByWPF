using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionChain
{
    public class User
    {
        public string UserName { get; set; }
    }

    public class UserTableEntity:TableEntity
    {
        public string UserName { get; set; }
    }
}
