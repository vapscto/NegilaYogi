using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using CommonLibrary;
namespace corewebapi18072016.Delegates
{
    public class StatusDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        private readonly FacadeUrl _config;
        private static FacadeUrl fdu = new FacadeUrl();
        CommonDelegate<CommonDTO, CommonDTO> COMMM = new CommonDelegate<CommonDTO, CommonDTO>();

        // empty Constructor, dnt delete please
        public StatusDelegate() { }

        public StatusDelegate(FacadeUrl config)
        {
            _config = config;
            fdu = config;
        }

        // get initial data
        public CommonDTO getInitailData(int mi_id)
        {

            return COMMM.GetDataById(mi_id, "StatusFacade/getinitialdata/");


            //CommonDTO rtnData = null;
            //string product;
            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri(fdu.StudentFacadeUrlTwo);
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            ////HTTP POST
            //try
            //{
            //    var myContent = JsonConvert.SerializeObject(mi_id);
            //    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            //    var byteContent = new ByteArrayContent(buffer);
            //    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //    var response = client.GetAsync("api/StatusFacade/getinitialdata/" + mi_id).Result;
            //    if (response.IsSuccessStatusCode)
            //    {
            //        product = response.Content.ReadAsStringAsync().Result;
            //        rtnData = JsonConvert.DeserializeObject<CommonDTO>(product, new JsonSerializerSettings
            //        {
            //            TypeNameHandling = TypeNameHandling.Objects
            //        });
            //    }
            //}
            //catch (Exception ee)
            //{
            //    Console.WriteLine(ee.Message);
            //}
            //return rtnData;
        }

        // Search student on search filter
        public CommonDTO getStudentOnSearchFilter(CommonDTO cdto)
        {
            return COMMM.POSTData(cdto, "StatusFacade/getdataonsearchfilter/");

            //CommonDTO rtnData = null;
            //string product;
            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri(fdu.StudentFacadeUrlTwo);
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            //try
            //{
            //    var myContent = JsonConvert.SerializeObject(cdto);
            //    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            //    var byteContent = new ByteArrayContent(buffer);
            //    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //    var response = client.PostAsync("api/StatusFacade/getdataonsearchfilter/", byteContent).Result;
            //    if (response.IsSuccessStatusCode)
            //    {
            //        product = response.Content.ReadAsStringAsync().Result;
            //        rtnData = JsonConvert.DeserializeObject<CommonDTO>(product, new JsonSerializerSettings
            //        {
            //            TypeNameHandling = TypeNameHandling.Objects
            //        });
            //    }
            //}
            //catch (Exception ee)
            //{
            //    Console.WriteLine(ee.Message);
            //}
            //return rtnData;
        }

        // update student changed status
        public CommonDTO saveData(CommonDTO cdto)
        {
            return COMMM.POSTData(cdto, "StatusFacade/savedata/");

            //string product;
            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri(fdu.StudentFacadeUrlTwo);
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            ////HTTP POST
            //try
            //{
            //    var myContent = JsonConvert.SerializeObject(cdto);
            //    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

            //    var byteContent = new ByteArrayContent(buffer);
            //    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //    var response = client.PostAsync("api/StatusFacade/savedata/", byteContent).Result;
            //    if (response.IsSuccessStatusCode)
            //    {
            //        product = response.Content.ReadAsStringAsync().Result;
            //        //cdto = JsonConvert.DeserializeObject<CommonDTO>(product, new JsonSerializerSettings
            //        //{
            //        //    TypeNameHandling = TypeNameHandling.Objects
            //        //});
            //    }
            //}
            //catch (Exception ee)
            //{
            //    Console.WriteLine(ee.Message);
            //}
            //return cdto;
        }
    }
}
