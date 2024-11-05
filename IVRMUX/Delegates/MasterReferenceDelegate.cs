using Newtonsoft.Json;
using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class MasterReferenceDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterRefernceDTO, MasterRefernceDTO> COMMM = new CommonDelegate<MasterRefernceDTO, MasterRefernceDTO>();
        public string getData(long resource)
        {
            string product = "";
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

            // HTTP GET
            try
            {
                HttpResponseMessage response = client.GetAsync("api/PreadmissionFacade/" + resource).Result;
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
        public MasterRefernceDTO savedetails(MasterRefernceDTO org)
        {

            return COMMM.POSTData(org, "MasterReferenceFacade/");
        }



        public MasterRefernceDTO GetmasterReferendetails(int id)
        {
            MasterRefernceDTO dto = null;
            return COMMM.GetDataByIdNo(id, dto, "MasterReferenceFacade/");
        }
        
        public MasterRefernceDTO DeleteMasterRecord(int id)
        {

            return COMMM.GetDataById(id, "MasterReferenceFacade/Deletedetails/");
        }
        public MasterRefernceDTO EditDetails(int id)
        {
            return COMMM.GetDataById(id, "MasterReferenceFacade/Editdetails/");
        }

        public MasterRefernceDTO getsearchdata(int data, MasterRefernceDTO dataa)
        {
            return COMMM.GETSEarchData(data, dataa, "MasterReferenceFacade/1/");
        }

    }
}
