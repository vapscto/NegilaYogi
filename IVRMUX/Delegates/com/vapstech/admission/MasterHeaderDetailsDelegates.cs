using System;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class MasterHeaderDetailsDelegates
    {


        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterHeaderDetailsDTO, MasterHeaderDetailsDTO> COMMM = new CommonDelegate<MasterHeaderDetailsDTO, MasterHeaderDetailsDTO>();
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

        public MasterHeaderDetailsDTO GetMasterHeaderDetailsData(MasterHeaderDetailsDTO lo)
        {

            return COMMM.POSTDataADM(lo, "MasterHeaderDetailsFacade/Getdetails/");

          
        }

        public MasterHeaderDetailsDTO SaveData(MasterHeaderDetailsDTO MasterHeaderDetailsDTO)
        {
            return COMMM.POSTDataADM(MasterHeaderDetailsDTO, "MasterHeaderDetailsFacade/SaveData");

         

        }

        public MasterHeaderDetailsDTO GetSelectedRowDetails(MasterHeaderDetailsDTO dto)
        {

            return COMMM.POSTDataADM(dto, "MasterHeaderDetailsFacade/GetSelectedRowDetails/");

            
        }
        public MasterHeaderDetailsDTO getmodulePage(MasterHeaderDetailsDTO dto)
        {
            return COMMM.POSTDataADM(dto, "MasterHeaderDetailsFacade/getmodulePage/");

        }

        public MasterHeaderDetailsDTO DeleteEntry(int ID)//Int32 IVRMM_Id
        {
            return COMMM.DeleteDataByIdADM(ID, "MasterHeaderDetailsFacade/DeleteEntry/");

          
        }

    }
}
