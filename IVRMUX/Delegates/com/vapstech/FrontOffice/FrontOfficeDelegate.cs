using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.BirthDay;
//using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.FrontOffice
{
    public class FrontOfficeDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<BirthDayDTO, BirthDayDTO> COMFRNT = new CommonDelegate<BirthDayDTO, BirthDayDTO>();
       
        public BirthDayDTO getdata(int id)
        {
            BirthDayDTO returnData = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:50642/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/FrontOfficeMonthEndReportFacade/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    returnData = JsonConvert.DeserializeObject<BirthDayDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects,
                        DateFormatHandling = DateFormatHandling.IsoDateFormat
                    });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return returnData;
        }

        public BirthDayDTO getmonthreport(BirthDayDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "FrontOfficeMonthEndReportFacade/getmonthreport/");
        }

    }
}

