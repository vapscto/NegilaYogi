using System;
using System.Net.Http;
using PreadmissionDTOs.com.vaps.admission;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class EMAILSMSTemplateSettingDelegates
    {

        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<EMAILSMSTemplateSettingDTO, EMAILSMSTemplateSettingDTO> COMMM = new CommonDelegate<EMAILSMSTemplateSettingDTO, EMAILSMSTemplateSettingDTO>();
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
                HttpResponseMessage response = client.GetAsync("api/EMAILSMSTemplateSettingFacadeController/" + resource).Result;
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

        public EMAILSMSTemplateSettingDTO Getdetails(EMAILSMSTemplateSettingDTO lo)
        {
            return COMMM.POSTDataADM(lo, "EMAILSMSTemplateSettingFacade/Getdetails");

          
        }

        public EMAILSMSTemplateSettingDTO GetSelectedRowDetails(int ID)//Int32 AMA_Id
        {
            return COMMM.GetDataByIdADM(ID, "EMAILSMSTemplateSettingFacade/GetSelectedRowDetails/");

        }

        public EMAILSMSTemplateSettingDTO MasterActivityData(EMAILSMSTemplateSettingDTO EMAILSMSTemplateSettingDTO)//Int32 IVRMM_Id
        {

            return COMMM.POSTDataADM(EMAILSMSTemplateSettingDTO, "EMAILSMSTemplateSettingFacade/");

           
        }

        public EMAILSMSTemplateSettingDTO MasterDeleteModulesData(int ID)//Int32 IVRMM_Id
        {

            return COMMM.DeleteDataByIdADM(ID, "EMAILSMSTemplateSettingFacade/MasterDeleteModulesDATA/");

         
        }

    }
}
