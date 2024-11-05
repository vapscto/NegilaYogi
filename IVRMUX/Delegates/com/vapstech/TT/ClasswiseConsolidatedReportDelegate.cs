using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.TT
{
 
    public class ClasswiseConsolidatedReportDelegate
    {
        //CommonDelegate<TT_ClasswiseConsolidatedReportDTO, TT_ClasswiseConsolidatedReportDTO> comm = new CommonDelegate<TT_ClasswiseConsolidatedReportDTO, TT_ClasswiseConsolidatedReportDTO>();
        //public TT_ClasswiseConsolidatedReportDTO loaddata(TT_ClasswiseConsolidatedReportDTO data)
        //{
        //    return comm.POSTDataTimeTable(data, "ClasswiseConsolidatedReportFacade/loaddata");
        //}
        public TT_ClasswiseConsolidatedReportDTO getalldetails(int id)
        {
            TT_ClasswiseConsolidatedReportDTO categorypage = null;
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:52949/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/ClasswiseConsolidatedReportFacade/getalldetails/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);
                    categorypage = JsonConvert.DeserializeObject<TT_ClasswiseConsolidatedReportDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return categorypage;
        }




        public TT_ClasswiseConsolidatedReportDTO Report(TT_ClasswiseConsolidatedReportDTO categorypage)
        {
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:52949/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(categorypage);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/ClasswiseConsolidatedReportFacade/Report", byteContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);
                    categorypage = JsonConvert.DeserializeObject<TT_ClasswiseConsolidatedReportDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch
            {

            }
            return categorypage;
        }
        public TT_ClasswiseConsolidatedReportDTO getabreport(TT_ClasswiseConsolidatedReportDTO categorypage)
        {
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:52949/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(categorypage);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/ClasswiseConsolidatedReportFacade/getabreport", byteContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);
                    categorypage = JsonConvert.DeserializeObject<TT_ClasswiseConsolidatedReportDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch
            {

            }
            return categorypage;
        }

    }
}
