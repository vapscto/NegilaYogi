using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Admission
{
    public class CollegemastercasteDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<CollegemastercasteDTO, CollegemastercasteDTO> COMMM = new CommonDelegate<CollegemastercasteDTO, CollegemastercasteDTO>();
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
                HttpResponseMessage response = client.GetAsync("api/CollegemastercasteFacade/" + resource).Result;
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

        public CollegemastercasteDTO GetmastercasteData(CollegemastercasteDTO lo)
        {
            return COMMM.clgadmissionbypost(lo, "CollegemastercasteFacade/Getdetails");            
        }

        public CollegemastercasteDTO GetSelectedRowDetails(int ID)//Int32 AMA_Id
        {
            return COMMM.clgadmissionbyid(ID, "CollegemastercasteFacade/GetSelectedRowDetails/");            
        }

        public CollegemastercasteDTO mastercasteData(CollegemastercasteDTO mastercasteDTO)//Int32 IVRMM_Id
        {
            return COMMM.clgadmissionbypost(mastercasteDTO, "CollegemastercasteFacade/");
        }

        public CollegemastercasteDTO MasterDeleteModulesData(int ID)//Int32 IVRMM_Id
        {
            return COMMM.clgadmissionbyid(ID, "CollegemastercasteFacade/MasterDeleteModulesDATA/");
        }

    }
}
