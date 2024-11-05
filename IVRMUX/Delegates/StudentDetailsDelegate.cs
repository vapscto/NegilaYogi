using Newtonsoft.Json;
using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates
{
    public class StudentDetailsDelegate 
    {
        public PointsReportDTO getdeatils(PointsReportDTO data)
        {
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(data);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/StudentDetailsFacade/getdetails/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    data = JsonConvert.DeserializeObject<PointsReportDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            // return output;
            return data;
        }

        public PointsReportDTO Getreportdetails(PointsReportDTO data)
        {
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(data);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/StudentDetailsFacade/Getreportdetails/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    data = JsonConvert.DeserializeObject<PointsReportDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            // return output;
            return data;
        }

    }
}
