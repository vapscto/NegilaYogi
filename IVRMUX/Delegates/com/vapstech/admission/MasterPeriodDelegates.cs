using System;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class MasterPeriodDelegates
    {


        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterPeriodDTO, MasterPeriodDTO> COMMM = new CommonDelegate<MasterPeriodDTO, MasterPeriodDTO>();
        public string getData(long resource)
        {
            string product = "";
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:57606/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // HTTP GET
            try
            {
                HttpResponseMessage response = client.GetAsync("api/AttendanceEntryTypeFacade/" + resource).Result;
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

        public MasterPeriodDTO GetMasterPeriodData(MasterPeriodDTO lo)
        {

            return COMMM.POSTDataADM(lo, "MasterPeriodFacade/Getdetails/");

          
        }

        public MasterPeriodDTO SaveData(MasterPeriodDTO MasterPeriodDTO)
        {
            return COMMM.POSTDataADM(MasterPeriodDTO, "MasterPeriodFacade/");

         

        }

        public MasterPeriodDTO GetSelectedRowDetails(MasterPeriodDTO dto)//Int32 IVRMM_Id
        {

            return COMMM.POSTDataADM(dto, "MasterPeriodFacade/GetSelectedRowDetails/");

            
        }

        public MasterPeriodDTO DeleteEntry(int ID)//Int32 IVRMM_Id
        {
            return COMMM.GetDataByIdADM(ID, "MasterPeriodFacade/DeleteEntry/");

          
        }

    }
}
