using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Scholorship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VidyaBharathi
{
    public class IVRM_Master_ViddyBharthiDelegate
    {
        CommonDelegate<ScholorshipMasterDTO, ScholorshipMasterDTO> _comm = new CommonDelegate<ScholorshipMasterDTO, ScholorshipMasterDTO>();
        CommonDelegate<ScholorshipStateDTO, ScholorshipStateDTO> _state = new CommonDelegate<ScholorshipStateDTO, ScholorshipStateDTO>();
        CommonDelegate<ScholorshipDitictDTO, ScholorshipDitictDTO> _distct = new CommonDelegate<ScholorshipDitictDTO, ScholorshipDitictDTO>();
        CommonDelegate<ScholorshipTalukaDTO, ScholorshipTalukaDTO> _taluka = new CommonDelegate<ScholorshipTalukaDTO, ScholorshipTalukaDTO>();
        public ScholorshipMasterDTO getalldetails(ScholorshipMasterDTO enqdto)
        {
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(enqdto);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/IVRM_Master_ViddyBharthi/getall/", byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    enqdto = JsonConvert.DeserializeObject<ScholorshipMasterDTO>(product, new JsonSerializerSettings
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

        public ScholorshipMasterDTO savecountry(ScholorshipMasterDTO id)
        {
            return _comm.POSTData(id, "IVRM_Master_ViddyBharthi/savecountry");
        }
        public ScholorshipStateDTO savestate(ScholorshipStateDTO id)
        {           //_state
            return _state.POSTData(id, "IVRM_Master_ViddyBharthi/savestate");
        }
        public ScholorshipDitictDTO onchnagestate(ScholorshipDitictDTO id)
        {
            return _distct.POSTData(id, "IVRM_Master_ViddyBharthi/onchnagestate");

        }
        public ScholorshipDitictDTO saveDistrict(ScholorshipDitictDTO id)
        {
            return _distct.POSTData(id, "IVRM_Master_ViddyBharthi/saveDistrict");
        }
        public ScholorshipTalukaDTO savetaluka(ScholorshipTalukaDTO id)
        {
            //_taluka
            return _taluka.POSTData(id, "IVRM_Master_ViddyBharthi/savetaluka");

        }
        public ScholorshipMasterDTO deactivateCountry(ScholorshipMasterDTO id)
        {
            return _comm.POSTData(id, "IVRM_Master_ViddyBharthi/deactivateCountry");
        }
        public ScholorshipStateDTO deactivestate(ScholorshipStateDTO id)
        {
            return _state.POSTData(id, "IVRM_Master_ViddyBharthi/deactivestate");
        }
        public ScholorshipDitictDTO deactivedistict(ScholorshipDitictDTO id)
        {
            return _distct.POSTData(id, "IVRM_Master_ViddyBharthi/deactivedistict");

        }
        //deactivetaluka
        public ScholorshipTalukaDTO deactivetaluka(ScholorshipTalukaDTO id)
        {
            return _taluka.POSTData(id, "IVRM_Master_ViddyBharthi/deactivetaluka");

        }
    }
}
