using System;
using System.Net.Http;
using PreadmissionDTOs.com.vaps.admission;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class MasterActivityDelegates
    {

        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterActivityDTO, MasterActivityDTO> COMMM = new CommonDelegate<MasterActivityDTO, MasterActivityDTO>();
        public string getData(long resource)
        {
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

            // HTTP GET
            try
            {
                HttpResponseMessage response = client.GetAsync("api/MasterActivityFacadeController/" + resource).Result;
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

        public MasterActivityDTO GetMasterActivityData(MasterActivityDTO lo)
        {
            return COMMM.GETDataADm(lo, "MasterActivityFacade/Getdetails");

          
        }

        public MasterActivityDTO GetSelectedRowDetails(int ID)//Int32 AMA_Id
        {
            return COMMM.GetDataByIdADM(ID, "MasterActivityFacade/GetSelectedRowDetails/");

        }

        public MasterActivityDTO MasterActivityData(MasterActivityDTO MasterActivityDTO)//Int32 IVRMM_Id
        {

            return COMMM.POSTDataADM(MasterActivityDTO, "MasterActivityFacade/");

           
        }

        public MasterActivityDTO MasterDeleteModulesData(int ID)//Int32 IVRMM_Id
        {

            return COMMM.GetDataByIdADM(ID, "MasterActivityFacade/MasterDeleteModulesDATA/");

         
        }

    }
}
