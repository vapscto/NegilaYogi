using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.TT.College
{
    public class CLGTTConstraintReportDelegate
    {
        CommonDelegate<CLGTTConstraintReportDTO, CLGTTConstraintReportDTO> comm = new CommonDelegate<CLGTTConstraintReportDTO, CLGTTConstraintReportDTO>();

        //public CLGTTConstraintReportDTO getalldetails()


        public CLGTTConstraintReportDTO getalldetails(int id)
        {
            CLGTTConstraintReportDTO categorypage = null;
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
                var response = client.GetAsync("api/CLGTTConstraintReportFacade/getalldetails/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);
                    categorypage = JsonConvert.DeserializeObject<CLGTTConstraintReportDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return categorypage;
        }


        //public CLGTTConstraintReportDTO getpagedetails(CLGTTConstraintReportDTO id)
        //{
        //    CLGTTConstraintReportDTO pageedit = null;
        //    string pagedetails;
        //    Array[] dropDownArray = new Array[2];
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("http://localhost:52949/");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    //HTTP POST
        //    try
        //    {
        //        var myContent = JsonConvert.SerializeObject(id);
        //        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

        //        var byteContent = new ByteArrayContent(buffer);
        //        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //        var response = client.PostAsync("api/CLGTTConstraintReportFacade/getpagedetails", byteContent).Result;

        //        if (response.IsSuccessStatusCode)
        //        {
        //            pagedetails = response.Content.ReadAsStringAsync().Result;

        //            pageedit = JsonConvert.DeserializeObject<CLGTTConstraintReportDTO>(pagedetails, new JsonSerializerSettings
        //            {
        //                TypeNameHandling = TypeNameHandling.Objects
        //            });
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //    }
        //    return pageedit;
        //}


        public CLGTTConstraintReportDTO getpagedetails(CLGTTConstraintReportDTO id)
        {
            return comm.POSTDataTimeTable(id, "CLGTTConstraintReportFacade/getpagedetails");
        }

    }
}
