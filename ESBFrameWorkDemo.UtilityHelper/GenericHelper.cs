using Microsoft.XLANGs.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESBFrameWorkDemo.UtilityHelper
{
    [Serializable]
    public static class GenericHelper
    {
        public static void SetEndpoint(Dictionary<string, string> resolverDictionary, XLANGMessage message)
        {
            try
            {
                string transportLocation = resolverDictionary["Resolver.TransportLocation"];
                //string outboundTransportCLSID = resolverDictionary["Resolver.OutboundTransportCLSID"];
                string endpointConfig = resolverDictionary["Resolver.EndpointConfig"];
                string transportType = resolverDictionary["Resolver.TransportType"];
                string action = resolverDictionary["Resolver.Action"];

                message.SetPropertyValue(typeof(BTS.OutboundTransportLocation), transportLocation);
                message.SetPropertyValue(typeof(BTS.OutboundTransportType), transportType);
                message.SetPropertyValue(typeof(BTS.Operation), action);

                if (!string.IsNullOrEmpty(endpointConfig))
                {
                    // parse delimited endpointconfig and set SFTP specific adapter properties
                    // endPointConfig data with this format "Key1=Value1;Key2=Value2;...."
                    var config = endpointConfig.Split('&').Select(part => part.Split('='))
                           .ToDictionary(split => split[0], split => split[1]);
                    // Set the context for the WCF-WebHttp adapter                   
                    message.SetPropertyValue(typeof(WCF.SecurityMode), config["SecurityMode"]);
                    message.SetPropertyValue(typeof(WCF.SuppressMessageBodyForHttpVerbs), config["SuppressMessageBodyForHttpVerbs"]);
                   // message.SetPropertyValue(typeof(WCF.HttpMethodAndUrl), config["HttpMethodAndUrl"]);
                }

            }
            catch (Exception ex)
            {

            }
        }
    }
}
