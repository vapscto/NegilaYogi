using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CommonLibrary;


namespace corewebapi18072016.Delegates.com.vapstech.Portals.Employee
{
    public class EmployeeDashboardDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<EmployeeDashboardDTO, EmployeeDashboardDTO> COMMM = new CommonDelegate<EmployeeDashboardDTO, EmployeeDashboardDTO>();

        public EmployeeDashboardDTO getalldetails(EmployeeDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeePtalFacade/getdata/");
        }
        public EmployeeDashboardDTO saveakpkfile(EmployeeDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeePtalFacade/saveakpkfile/");
        }
        public EmployeeDashboardDTO viewnotice(EmployeeDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeePtalFacade/viewnotice/");
        }

        public EmployeeDashboardDTO onclick_notice(EmployeeDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeePtalFacade/onclick_notice/");
        }

        public EmployeeDashboardDTO onclick_events(EmployeeDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeePtalFacade/onclick_events/");
        }

        public EmployeeDashboardDTO onclick_asset(EmployeeDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeePtalFacade/onclick_asset/");
        }
        //public EmployeeDashboardDTO getalldetails(EmployeeDashboardDTO data)
        //{

        //    //string product;
        //    //HttpClient client = new HttpClient();
        //    //client.BaseAddress = new Uri("http://localhost:51263/");
        //    //client.DefaultRequestHeaders.Accept.Clear();
        //    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    ////HTTP POST
        //    //try
        //    //{
        //    //    var myContent = JsonConvert.SerializeObject(data);
        //    //    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
        //    //    var byteContent = new ByteArrayContent(buffer);
        //    //    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //    //    var response = client.PostAsync("api/EmployeeDashboardFacade/getalldetails", byteContent).Result;
        //    //    if (response.IsSuccessStatusCode)
        //    //    {
        //    //        product = response.Content.ReadAsStringAsync().Result;
        //    //        Console.WriteLine("", product);
        //    //        data = JsonConvert.DeserializeObject<EmployeeDashboardDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        //    //    }
        //    //}
        //    //catch (Exception e)
        //    //{
        //    //    throw e;
        //    //}
        //    //return data;

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
        //        var response = client.PostAsync("api/EmployeePtalFacade/getdata/", byteContent).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            product = response.Content.ReadAsStringAsync().Result;
        //            Console.WriteLine("", product);
        //            data = JsonConvert.DeserializeObject<EmployeeDashboardDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return data;

        //}
    }
}
