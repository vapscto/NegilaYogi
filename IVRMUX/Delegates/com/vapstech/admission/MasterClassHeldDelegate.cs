using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CommonLibrary;
namespace corewebapi18072016.Delegates.com.vapstech.admission
{
    public class MasterClassHeldDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterClassHeldDTO, MasterClassHeldDTO> COMMM = new CommonDelegate<MasterClassHeldDTO, MasterClassHeldDTO>();
        public string getData(long resource)
        {
            string product = "";
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // HTTP GET
            try
            {
                HttpResponseMessage response = client.GetAsync("api/MasterClassHeldFacade/" + resource).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("{0}\t${1}\t{2}", product);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetBaseException());
            }
            return product;
        }
        public MasterClassHeldDTO getaldata(MasterClassHeldDTO id)
        {
            return COMMM.POSTDataADM(id, "MasterClassHeldFacade/getDetails/");
        }
        public MasterClassHeldDTO saveData(MasterClassHeldDTO dto)
        {
            return COMMM.POSTDataADM(dto, "MasterClassHeldFacade/");
        }
        public MasterClassHeldDTO getNoOfClassHeld(MasterClassHeldDTO dto)
        {
            return COMMM.POSTDataADM(dto, "MasterClassHeldFacade/getNoOfClassHeld/");
        }


    }
}
