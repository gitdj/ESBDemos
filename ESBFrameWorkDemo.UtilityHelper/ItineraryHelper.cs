using Microsoft.Practices.ESB.Itinerary;
using Microsoft.Practices.ESB.Itinerary.OM.V1;
using Microsoft.Practices.ESB.Resolver;
using Microsoft.XLANGs.BaseTypes;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;


namespace ESBFrameWorkDemo.ESB.Components
{
    [Serializable]
    public class ItineraryHelper
    {
        public void InitializeAndWrite(XLANGMessage msg, string config)
        {

            try
            {
                //Trace
                System.Diagnostics.Trace.WriteLine("[itHero.ESB.ItineraryV1Helper] v1.4");

                // Lookup itinerary via resolver
                ResolverDictionary dictionary = ResolverMgr.Resolve(config);

                string uuid = dictionary.BaseDictionary.ContainsKey("Resolver.ItineraryUUID") ? dictionary.BaseDictionary["Resolver.ItineraryUUID"] : Guid.NewGuid().ToString();
                object correlationToken = msg.GetPropertyValue(typeof(BTS.CorrelationToken));
                object reqRespTransmitPipelineID = msg.GetPropertyValue(typeof(BTS.ReqRespTransmitPipelineID));
                object interchangeID = msg.GetPropertyValue(typeof(BTS.InterchangeID));
                object messageID = msg.GetPropertyValue(typeof(BTS.MessageID));
                object epmRRCorrelationToken = msg.GetPropertyValue(typeof(BTS.EpmRRCorrelationToken));
                epmRRCorrelationToken = epmRRCorrelationToken + "|" + correlationToken + "|" + reqRespTransmitPipelineID;

                //Trace
                Trace.WriteLine("[itHero.ESB.ItineraryV1Helper] correlationToken: " + correlationToken);
                Trace.WriteLine("[itHero.ESB.ItineraryV1Helper] reqRespTransmitPipelineID: " + reqRespTransmitPipelineID);
                Trace.WriteLine("[itHero.ESB.ItineraryV1Helper] interchangeID: " + interchangeID);
                Trace.WriteLine("[itHero.ESB.ItineraryV1Helper] messageID: " + messageID);
                Trace.WriteLine("[itHero.ESB.ItineraryV1Helper] epmRRCorrelationToken: " + epmRRCorrelationToken);

                // Serialize itinerary
                StringReader reader = new StringReader(dictionary.Item("Resolver.Itinerary"));
                XmlSerializer ser = new XmlSerializer(typeof(Itinerary), "http://schemas.microsoft.biztalk.practices.esb.com/itinerary");

                //Create Itinerary object
                Itinerary it = (Itinerary)ser.Deserialize(reader);

                //Get the first service from itinerary
                ItineraryServicesService service = it.Services[0].Service;
               
                //Trace
                System.Diagnostics.Trace.WriteLine("[itHero.ESB.ItineraryV1Helper] itineraryStep.ItineraryStep.Id: " + service.id);
                System.Diagnostics.Trace.WriteLine("[itHero.ESB.ItineraryV1Helper] itineraryStep.ItineraryStep.ServiceName: " + service.name);
                System.Diagnostics.Trace.WriteLine("[itHero.ESB.ItineraryV1Helper] itineraryStep.ItineraryStep.ServiceType: " + service.type);
                System.Diagnostics.Trace.WriteLine("[itHero.ESB.ItineraryV1Helper] itineraryStep.ItineraryStep.State: " + service.stage);                               

                //Create ItineraryV1
                Microsoft.Practices.ESB.Itinerary.OM.V1.ItineraryV1 itv1 = new ItineraryV1();

                itv1.Read(dictionary.Item("Resolver.Itinerary"));

                if (string.IsNullOrEmpty(itv1.ItineraryData.uuid))
                {
                    itv1.ItineraryData.uuid = uuid;
                }

                itv1.ItineraryData.beginTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture);
                itv1.ItineraryData.completeTime = "";
                itv1.ItineraryData.servicecount = itv1.ItineraryData.Services.Length;                

                if (itv1.ItineraryData.BizTalkSegment == null)
                {
                    itv1.ItineraryData.BizTalkSegment = new ItineraryBizTalkSegment();
                }

                itv1.ItineraryData.BizTalkSegment.epmRRCorrelationToken = string.IsNullOrEmpty((string)epmRRCorrelationToken) ? "" : epmRRCorrelationToken.ToString();
                itv1.ItineraryData.BizTalkSegment.receiveInstanceId = "";
                itv1.ItineraryData.BizTalkSegment.messageId = string.IsNullOrEmpty((string)epmRRCorrelationToken) ? "" : epmRRCorrelationToken.ToString();
                itv1.ItineraryData.BizTalkSegment.interchangeId = string.IsNullOrEmpty((string)interchangeID) ? "" : interchangeID.ToString();

                //Write properties               
                msg.SetPropertyValue(typeof(Microsoft.Practices.ESB.Itinerary.Schemas.ServiceName), service.name);
                msg.SetPropertyValue(typeof(Microsoft.Practices.ESB.Itinerary.Schemas.ServiceType), ItineraryServiceType.Orchestration);
                msg.SetPropertyValue(typeof(Microsoft.Practices.ESB.Itinerary.Schemas.ServiceState), service.state);
                msg.SetPropertyValue(typeof(Microsoft.Practices.ESB.Itinerary.Schemas.IsRequestResponse), false);

                //Trace
                System.Diagnostics.Trace.WriteLine("[itHero.ESB.ItineraryV1Helper] Writed ESB properties in XLANGMessage ");

                //Set ItineraryHeader property value in XLANGMessage
                itv1.Write(msg);

                //Trace
                System.Diagnostics.Trace.WriteLine("[itHero.ESB.ItineraryV1Helper] Writed Itinerary in XLANGMessage ");

            }
            catch (Exception ex)
            {
                Trace.WriteLine("[itHero.ESB.ItineraryV1Helper] Exception: " + ex.Message);
            }
        }
    }
}
