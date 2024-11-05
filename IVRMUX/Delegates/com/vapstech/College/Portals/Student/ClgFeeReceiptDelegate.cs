using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.College.Portals;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.College.Fees;

namespace corewebapi18072016.Delegates.com.vapstech.Portals.Student
{
    public class ClgFeeReceiptDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ClgPortalFeeDTO, ClgPortalFeeDTO> COMMM = new CommonDelegate<ClgPortalFeeDTO, ClgPortalFeeDTO>();
        public ClgPortalFeeDTO getloaddata(ClgPortalFeeDTO data)
        {     
            return COMMM.CLGPortalPOSTData(data, "ClgFeeReceiptFacade/getloaddata/");
        }

        public CollegeFeeTransactionDTO getdetails(CollegeFeeTransactionDTO id)
        {
            CollegeFeeTransactionDTO enqdto = null;
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65159/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/ClgFeeReceiptFacade/printreceipt/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    enqdto = JsonConvert.DeserializeObject<CollegeFeeTransactionDTO>(product, new JsonSerializerSettings
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
        //public ClgPortalFeeDTO getdetails(ClgPortalFeeDTO sddto)
        //{
        //    return COMMM.CLGPortalPOSTData(sddto, "ClgFeeReceiptFacade/getdetails/");
        //}
        public ClgPortalFeeDTO getrecdetails(ClgPortalFeeDTO sddto)
        {
            return COMMM.CLGPortalPOSTData(sddto, "ClgFeeReceiptFacade/getrecdetails/");
        }

        public CollegeFeeTransactionDTO printrecdelegate(CollegeFeeTransactionDTO id)
        {
            CollegeFeeTransactionDTO enqdto = null;
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65159/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/ClgFeeReceiptFacade/printreceipt/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    enqdto = JsonConvert.DeserializeObject<CollegeFeeTransactionDTO>(product, new JsonSerializerSettings
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
    }
}
