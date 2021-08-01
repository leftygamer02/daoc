using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class ServerInfo : DataObjectBase
    {
        public string Time { get; set; }
        public string ServerName { get; set; }
        public string AAC { get; set; }
        public string ServerType { get; set; }
        public string ServerStatus { get; set; }
        public int NumClients { get; set; }
        public int NumAccounts { get; set; }
        public int NumMobs { get; set; }
        public int NumInventoryItems { get; set; }
        public int NumPlayerChars { get; set; }
        public int NumMerchantItems { get; set; }
        public int NumItemTemplates { get; set; }
        public int NumWorldObjects { get; set; }

    }
}
