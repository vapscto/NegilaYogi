
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using Newtonsoft.Json;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class PrincipalDashboardDelegates
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<PrincipalDashboardDTO, PrincipalDashboardDTO> COMMM = new CommonDelegate<PrincipalDashboardDTO, PrincipalDashboardDTO>();

        //public PrincipalDashboardDTO Getdetails(PrincipalDashboardDTO data)
        //{

        //    string product;
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("http://localhost:51263/");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    //HTTP POST
        //    try
        //    {
        //        var myContent = JsonConvert.SerializeObject(data);
        //        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
        //        var byteContent = new ByteArrayContent(buffer);
        //        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //        var response = client.PostAsync("api/PrincipalDashboardFacade/Getdetails", byteContent).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            product = response.Content.ReadAsStringAsync().Result;
        //            Console.WriteLine("", product);
        //            data = JsonConvert.DeserializeObject<PrincipalDashboardDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return data;
        //}

        public PrincipalDashboardDTO Getdetails(PrincipalDashboardDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "PrincipalDashboardFacade/Getdetails/");
        }
        public PrincipalDashboardDTO onclick_notice(PrincipalDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "PrincipalDashboardFacade/onclick_notice/");
        }
        public PrincipalDashboardDTO viewnotice(PrincipalDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "PrincipalDashboardFacade/viewnotice/");
        }

    }
}
