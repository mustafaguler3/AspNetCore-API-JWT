using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdemyAuthServer.Core.Configuration
{
    public class Client
    {
        public string Id { get; set; }

        public string Secret { get; set; }
        //www.myapi1.com www.myapi2.com
        public List<String> Audiences { get; set; }//api ye kimler erişecek
    }
}
