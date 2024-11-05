using System;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs.com.vaps.admission;
using CommonLibrary;

namespace corewebapi18072016.com.vaps.admission.Delegates
{
    public class MasterDocumentDelegates
    {


        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterDocumentDTO, MasterDocumentDTO> COMMM = new CommonDelegate<MasterDocumentDTO, MasterDocumentDTO>();
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
                HttpResponseMessage response = client.GetAsync("api/MasterDocumentFacade/" + resource).Result;
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

        public MasterDocumentDTO Getdetails(MasterDocumentDTO lo)
        {
            return COMMM.POSTDataADM(lo, "MasterDocumentFacade/Getdetails");

        }

        public MasterDocumentDTO SaveData(MasterDocumentDTO MasterDocumentDTO)
        {
            return COMMM.POSTDataADM(MasterDocumentDTO, "MasterDocumentFacade/");

         

        }

        public MasterDocumentDTO GetSelectedRowDetails(int ID)//Int32 IVRMM_Id
        {
            return COMMM.GetDataByIdADM(ID, "MasterDocumentFacade/GetSelectedRowDetails/");



        }

        public MasterDocumentDTO DeleteData(int ID)//Int32 IVRMM_Id
        {

            return COMMM.GetDataByIdADM(ID, "MasterDocumentFacade/DeleteEntry/");

          

        }

    }
}
