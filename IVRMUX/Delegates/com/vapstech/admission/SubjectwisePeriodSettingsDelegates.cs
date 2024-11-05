using System;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class SubjectwisePeriodSettingsDelegates
    {


        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<SubjectwisePeriodSettingsDTO, SubjectwisePeriodSettingsDTO> COMMM = new CommonDelegate<SubjectwisePeriodSettingsDTO, SubjectwisePeriodSettingsDTO>();
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
                HttpResponseMessage response = client.GetAsync("api/SubjectwisePeriodSettingsFacade/" + resource).Result;
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

        public SubjectwisePeriodSettingsDTO GetData(SubjectwisePeriodSettingsDTO dto)
        {

            return COMMM.POSTDataADM(dto, "SubjectwisePeriodSettingsFacade/Getdetails/");

        }

        public SubjectwisePeriodSettingsDTO SaveData(SubjectwisePeriodSettingsDTO SubjectwisePeriodSettingsDTO)
        {
            return COMMM.POSTDataADM(SubjectwisePeriodSettingsDTO, "SubjectwisePeriodSettingsFacade/");
 

        }
        public SubjectwisePeriodSettingsDTO getsubjectMaxPeriod(SubjectwisePeriodSettingsDTO SubjectwisePeriodSettingsDTO)
        {
            return COMMM.POSTDataADM(SubjectwisePeriodSettingsDTO, "SubjectwisePeriodSettingsFacade/getsubjectMaxPeriod/");


        }
        



    }
}
