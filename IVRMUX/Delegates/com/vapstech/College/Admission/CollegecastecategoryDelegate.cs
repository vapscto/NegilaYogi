using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Admission
{
    public class CollegecastecategoryDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<CollegecastecategoryDTO, CollegecastecategoryDTO> COMMM = new CommonDelegate<CollegecastecategoryDTO, CollegecastecategoryDTO>();
        public string getData(long resource)
        {
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50790/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

            // HTTP GET
            try
            {
                HttpResponseMessage response = client.GetAsync("api/CollegecastecategoryFacade/" + resource).Result;
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

        public CollegecastecategoryDTO GetcastecategoryData(CollegecastecategoryDTO lo)
        {
            return COMMM.clgadmissionbypost(lo, "CollegecastecategoryFacade/Getdetails");
        }

        public CollegecastecategoryDTO GetSelectedRowDetails(int ID)//Int32 AMA_Id
        {
            return COMMM.clgadmissionbyid(ID, "CollegecastecategoryFacade/GetSelectedRowDetails/");
        }

        public CollegecastecategoryDTO castecategoryData(CollegecastecategoryDTO CollegecastecategoryDTO)//Int32 IVRMM_Id
        {
            return COMMM.clgadmissionbypost (CollegecastecategoryDTO, "CollegecastecategoryFacade/");
        }

        public CollegecastecategoryDTO MasterDeleteModulesData(int ID)//Int32 IVRMM_Id
        {
            return COMMM.clgadmissionbyid(ID, "CollegecastecategoryFacade/MasterDeleteModulesDATA/");
        }
    }
}
