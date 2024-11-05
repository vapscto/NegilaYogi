using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Portals.Student;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Fees;

namespace corewebapi18072016.Delegates.com.vapstech.Portals.Student
{
    public class FeeReceiptDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<StudentDashboardDTO, StudentDashboardDTO> COMMM = new CommonDelegate<StudentDashboardDTO, StudentDashboardDTO>();
        public StudentDashboardDTO getloaddata(StudentDashboardDTO data)
        {     
            return COMMM.POSTPORTALData(data, "FeeReceiptFacade/getloaddata/");
        }

        public FeeStudentTransactionDTO getdetails(FeeStudentTransactionDTO id)
        {
            FeeStudentTransactionDTO enqdto = null;
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:51263/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/FeeReceiptFacade/printreceipt/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    enqdto = JsonConvert.DeserializeObject<FeeStudentTransactionDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return enqdto;
        }
        //public StudentDashboardDTO getdetails(StudentDashboardDTO sddto)
        //{
        //    return COMMM.POSTPORTALData(sddto, "FeeReceiptFacade/getdetails/");
        //}
        public StudentDashboardDTO getrecdetails(StudentDashboardDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "FeeReceiptFacade/getrecdetails/");
        }

        public StudentDashboardDTO getstudetails(StudentDashboardDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "FeeReceiptFacade/getstudetails/");
        }

        public StudentDashboardDTO preadmissiongetrecdetails(StudentDashboardDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "FeeReceiptFacade/preadmissiongetrecdetails/");
        }
        public StudentDashboardDTO preadmissiongetdetails(StudentDashboardDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "FeeReceiptFacade/preadmissiongetdetails/");
        }
    }
}
