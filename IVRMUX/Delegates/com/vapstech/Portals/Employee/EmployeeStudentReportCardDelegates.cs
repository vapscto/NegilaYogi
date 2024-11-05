using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using Microsoft.AspNetCore.Mvc;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Student;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Delegates.com.vapstech.Portals.Employee
{
    [Route("api/[controller]")]
    public class EmployeeStudentReportCardDelegates : Controller
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<EmployeeDashboardDTO, EmployeeDashboardDTO> COMMM = new CommonDelegate<EmployeeDashboardDTO, EmployeeDashboardDTO>();
        CommonDelegate<OnlineLeaveApp_DTO, OnlineLeaveApp_DTO> COMMO = new CommonDelegate<OnlineLeaveApp_DTO, OnlineLeaveApp_DTO>();


      
       public EmployeeDashboardDTO Getdetails(EmployeeDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentReportCardFacade/Getdetails/");

            //EmployeeDashboardDTO DTO = new EmployeeDashboardDTO();
            //string product;
            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("http://localhost:51263/");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            ////HTTP POST
            //try
            //{

            //    var myContent = JsonConvert.SerializeObject(data);
            //    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            //    var byteContent = new ByteArrayContent(buffer);
            //    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //    var response = client.PostAsync("api/EmployeeStudentReportCardFacade/Getdetails", byteContent).Result;

            //    if (response.IsSuccessStatusCode)
            //    {
            //        product = response.Content.ReadAsStringAsync().Result;
            //        Console.WriteLine("", product);

            //        DTO = JsonConvert.DeserializeObject<EmployeeDashboardDTO>(product, new JsonSerializerSettings
            //        {
            //            TypeNameHandling = TypeNameHandling.Objects
            //        });
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //}
            //return DTO;
        }
        
        public EmployeeDashboardDTO showdetails(EmployeeDashboardDTO data)//Int32 IVRMM_Id
        {
            EmployeeDashboardDTO DTO = new EmployeeDashboardDTO();
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:51263/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {

                var myContent = JsonConvert.SerializeObject(data);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/EmployeeStudentReportCardFacade/showdetails", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    DTO = JsonConvert.DeserializeObject<EmployeeDashboardDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return DTO;

        }

        public EmployeeDashboardDTO get_class(EmployeeDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentReportCardFacade/get_class/");
        }
        public EmployeeDashboardDTO get_section(EmployeeDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentReportCardFacade/get_section/");
        }
        public EmployeeDashboardDTO get_student(EmployeeDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentReportCardFacade/get_student/");
        }
        public EmployeeDashboardDTO get_exam(EmployeeDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentReportCardFacade/get_exam/");
        }
        

    }
}
