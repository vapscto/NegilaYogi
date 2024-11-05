using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs.com.vaps.College.Fees;
using Newtonsoft.Json;
using System.Collections;
using System.IO;
using CommonLibrary;

namespace corewebapi18072016.Delegates.com.vapstech.College.Fees
{
    public class CLGFeeAdjustmentDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<CLGFeeAdjustmentDTO, CLGFeeAdjustmentDTO> COMMM = new CommonDelegate<CLGFeeAdjustmentDTO, CLGFeeAdjustmentDTO>();

        public CLGFeeAdjustmentDTO getdata(CLGFeeAdjustmentDTO data)
        {
            return COMMM.PostClgFee(data, "CLGFeeAdjustmentFacade/getalldetails/");           
        }

       
        public CLGFeeAdjustmentDTO getdataclass(CLGFeeAdjustmentDTO enqdto)
        {
            return COMMM.PostClgFee(enqdto, "CLGFeeAdjustmentFacade/getclass/");           
        }
        public CLGFeeAdjustmentDTO getdatasection(CLGFeeAdjustmentDTO data)
        {
            return COMMM.PostClgFee(data, "CLGFeeAdjustmentFacade/getsection/");           
        }
        public CLGFeeAdjustmentDTO getdatastudent(CLGFeeAdjustmentDTO data)
        {
            return COMMM.PostClgFee(data, "CLGFeeAdjustmentFacade/getstudent/");           
        }

        //5
        public CLGFeeAdjustmentDTO getdatabothgroup(CLGFeeAdjustmentDTO data)
        {
            return COMMM.PostClgFee(data, "CLGFeeAdjustmentFacade/getbothgroup/");          
        }

        public CLGFeeAdjustmentDTO getdatafromhead(CLGFeeAdjustmentDTO data)
        {
            return COMMM.PostClgFee(data, "CLGFeeAdjustmentFacade/getfromhead/");            
        }
        public CLGFeeAdjustmentDTO getdatatohead(CLGFeeAdjustmentDTO data)
        {
            return COMMM.PostClgFee(data, "CLGFeeAdjustmentFacade/gettohead/");           
        }

        public CLGFeeAdjustmentDTO savedatadelegate(CLGFeeAdjustmentDTO data)
        {
            return COMMM.PostClgFee(data, "CLGFeeAdjustmentFacade/savedata/");           
        }

        public CLGFeeAdjustmentDTO EditDetails(int id)
        {
            return COMMM.GETClgFee(id, "CLGFeeAdjustmentFacade/getpagedetails/");           
        }
        public CLGFeeAdjustmentDTO deactivate(CLGFeeAdjustmentDTO id)
        {
            return COMMM.PostClgFee(id, "CLGFeeAdjustmentFacade/deactivate/");           
        }
        public CLGFeeAdjustmentDTO deleterec(int id)
        {
            CLGFeeAdjustmentDTO enqdto = null;
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:55570/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.DeleteAsync("api/CLGFeeAdjustmentFacade/deletedetails/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    enqdto = JsonConvert.DeserializeObject<CLGFeeAdjustmentDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });

                    enqdto = JsonConvert.DeserializeObject<CLGFeeAdjustmentDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            // return output;
            return enqdto;
        }
        public CLGFeeAdjustmentDTO searching(CLGFeeAdjustmentDTO id)
        {
            return COMMM.PostClgFee(id, "CLGFeeAdjustmentFacade/searching");           
        }
    }
}
