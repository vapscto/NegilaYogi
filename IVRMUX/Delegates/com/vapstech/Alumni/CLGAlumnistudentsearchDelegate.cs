using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Alumni;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Alumni
{
    public class CLGAlumnistudentsearchDelegate 
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<CLGAlumniStudentDTO, CLGAlumniStudentDTO> COMMM = new CommonDelegate<CLGAlumniStudentDTO, CLGAlumniStudentDTO>();
        public CLGAlumniStudentDTO getData(CLGAlumniStudentDTO stu)
        {

            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:54176/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(stu);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/CLGAlumnistudentsearchFacade/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    stu = JsonConvert.DeserializeObject<CLGAlumniStudentDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stu;
        }

        public CLGAlumniStudentDTO getsemdata(CLGAlumniStudentDTO data)
        {
            return COMMM.POSTDataAlumni(data, "CLGAlumnistudentsearchFacade/getsemdata/");
        }
    }
}
