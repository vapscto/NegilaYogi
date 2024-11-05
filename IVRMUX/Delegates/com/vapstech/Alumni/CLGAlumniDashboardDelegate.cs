using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Alumni;

namespace IVRMUX.Delegates.com.vapstech.Alumni
{
    public class CLGAlumniDashboardDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<CLGAlumniStudentDTO, CLGAlumniStudentDTO> COMMM = new CommonDelegate<CLGAlumniStudentDTO, CLGAlumniStudentDTO>();

        public CLGAlumniStudentDTO getalldetails(CLGAlumniStudentDTO data)
        {
            return COMMM.POSTDataAlumni(data, "ALUDASHFacade/getloaddata/");
        }
        public CLGAlumniStudentDTO saveakpkfile(CLGAlumniStudentDTO data)
        {
            return COMMM.POSTDataAlumni(data, "CLGAlumniDashboardFacade/saveakpkfile/");
        }
        //public CLGAlumniStudentDTO getalldetails(CLGAlumniStudentDTO data)
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
        //    //        data = JsonConvert.DeserializeObject<CLGAlumniStudentDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
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
        //        var response = client.PostAsync("api/CLGAlumniDashboardFacade/getdata/", byteContent).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            product = response.Content.ReadAsStringAsync().Result;
        //            Console.WriteLine("", product);
        //            data = JsonConvert.DeserializeObject<CLGAlumniStudentDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
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
