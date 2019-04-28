using Microsoft.Practices.ESB.Adapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESB.WebHttpAdapterProvider
{
    public class WebHttpAdapterProvider : WCFBaseAdapterProvider
    {
        public override string AdapterName
        {
            get
            {
                return "WCF-WebHttp";
            }
        }
    }
}
