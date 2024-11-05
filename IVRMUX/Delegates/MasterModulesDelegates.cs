using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs;
using Newtonsoft.Json;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class MasterModulesDelegates
    {


        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<MasterModulesDTO, MasterModulesDTO> COMMM = new CommonDelegate<MasterModulesDTO, MasterModulesDTO>();

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
                HttpResponseMessage response = client.GetAsync("api/MasterModulesFacadeController/" + resource).Result;
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

        public MasterModulesDTO GetMasterModulesData(MasterModulesDTO lo)
        {
            return COMMM.GETData(lo, "MasterModulesFacade/Getdetails/");
        }

        public MasterModulesDTO GetSelectedRowDetails(int ID)//Int32 IVRMM_Id
        {
            return COMMM.GetDataById(ID, "MasterModulesFacade/GetSelectedRowDetails/");
            
        }

        public MasterModulesDTO MasterModulesData(MasterModulesDTO MasterModulesDTO)//Int32 IVRMM_Id
        {

            return COMMM.POSTData(MasterModulesDTO, "MasterModulesFacade/");
        }

        public MasterModulesDTO MasterDeleteModulesData(int ID)//Int32 IVRMM_Id
        {
            return COMMM.DeleteDataById(ID, "MasterModulesFacade/MasterDeleteModulesDATA/");

        }

    }
}
