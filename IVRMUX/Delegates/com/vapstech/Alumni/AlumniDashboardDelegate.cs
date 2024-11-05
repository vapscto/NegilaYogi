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
    public class AlumniDashboardDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<AlumniStudentDTO, AlumniStudentDTO> COMMM = new CommonDelegate<AlumniStudentDTO, AlumniStudentDTO>();

        public AlumniStudentDTO getalldetails(AlumniStudentDTO data)
        {
            return COMMM.POSTDataAlumni(data, "SchoolALUDASHFacade/getloaddata/");
        }
        public AlumniStudentDTO saveakpkfile(AlumniStudentDTO data)
        {
            return COMMM.POSTDataAlumni(data, "CLGAlumniDashboardFacade/saveakpkfile/");
        }
        public AlumniStudentDTO yearwiselist(AlumniStudentDTO data)
        {
            return COMMM.POSTDataAlumni(data, "SchoolALUDASHFacade/yearwiselist/");
        }
        public AlumniStudentDTO classwisestudent(AlumniStudentDTO data)
        {
            return COMMM.POSTDataAlumni(data, "SchoolALUDASHFacade/classwisestudent/");
        }
        public AlumniStudentDTO getgallery(AlumniStudentDTO data)
        {
            return COMMM.POSTDataAlumni(data, "SchoolALUDASHFacade/getgallery/");
        }
        public AlumniStudentDTO viewgallery(AlumniStudentDTO data)
        {
            return COMMM.POSTDataAlumni(data, "SchoolALUDASHFacade/viewgallery/");
        }
        public AlumniStudentDTO alumninotice(AlumniStudentDTO data)
        {
            return COMMM.POSTDataAlumni(data, "SchoolALUDASHFacade/alumninotice/");
        }
         public AlumniStudentDTO viewnotice(AlumniStudentDTO data)
        {
            return COMMM.POSTDataAlumni(data, "SchoolALUDASHFacade/viewnotice/");
        }

        //public AlumniStudentDTO getalldetails(AlumniStudentDTO data)
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
        //    //        data = JsonConvert.DeserializeObject<AlumniStudentDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
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
        //            data = JsonConvert.DeserializeObject<AlumniStudentDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
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
