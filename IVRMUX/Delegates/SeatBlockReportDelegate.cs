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
    public class SeatBlockReportDelegate
    {

        private const String JsonContentType = "application/json; charset=utf-8";


        //public SeatBlockReportDTO getstuddetails(SeatBlockReportDTO data)
        //{
        //    SeatBlockReportDTO enqdto = null;
        //    string product;
        //    Array[] dropDownArray = new Array[2];
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("http://localhost:65140/");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    //HTTP POST
        //    try
        //    {
        //        var myContent = JsonConvert.SerializeObject(data);
        //        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

        //        var byteContent = new ByteArrayContent(buffer);
        //        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //        var response = client.PostAsync("api/SeatBlockReportFacade/getstuddetails/", byteContent).Result;

        //        if (response.IsSuccessStatusCode)
        //        {
        //            product = response.Content.ReadAsStringAsync().Result;

        //            enqdto = JsonConvert.DeserializeObject<SeatBlockReportDTO>(product, new JsonSerializerSettings
        //            {
        //                TypeNameHandling = TypeNameHandling.Objects
        //            });
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //    }

        //    return enqdto;
        //}
        public SeatBlockReportDTO getdetails(SeatBlockReportDTO data)
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
                var response = client.PostAsync("api/SeatBlockReportFacade/getdetails/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    data = JsonConvert.DeserializeObject<SeatBlockReportDTO>(product, new JsonSerializerSettings
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

        public SeatBlockReportDTO Getstudlist(SeatBlockReportDTO data)
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
                var response = client.PostAsync("api/SeatBlockReportFacade/Getstudlist/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    data = JsonConvert.DeserializeObject<SeatBlockReportDTO>(product, new JsonSerializerSettings
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

        public SeatBlockReportDTO Getreportdetails(SeatBlockReportDTO data)
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
                var response = client.PostAsync("api/SeatBlockReportFacade/Getreportdetails/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    data = JsonConvert.DeserializeObject<SeatBlockReportDTO>(product, new JsonSerializerSettings
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
