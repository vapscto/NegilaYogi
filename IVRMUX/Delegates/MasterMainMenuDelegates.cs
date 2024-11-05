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
    public class MasterMainMenuDelegates
    {


        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterMainMenuDTO, MasterMainMenuDTO> COMMM = new CommonDelegate<MasterMainMenuDTO, MasterMainMenuDTO>();
        public string getData(long resource)
        {



            string product = "";
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

            try
            {
                HttpResponseMessage response = client.GetAsync("api/MasterMainMenuFacadeController/" + resource).Result;
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

        public MasterMainMenuDTO GetMasterMainMenuData(MasterMainMenuDTO lo)
        {
            return COMMM.GETData(lo, "MasterMainMenuFacade/Getdetails");
        }

        public MasterMainMenuDTO GetSelectedRowDetails(int ID)
        {
            return COMMM.GetDataById(ID, "MasterMainMenuFacade/GetSelectedRowDetails/");
        }

        public MasterMainMenuDTO MasterMainMenuDTO(MasterMainMenuDTO MasterMainMenuDTO)//Int32 IVRMM_Id
        {
            return COMMM.POSTData(MasterMainMenuDTO, "MasterMainMenuFacade/");
        }

        public MasterMainMenuDTO MasterDeleteMainMenuDTO(int ID)
        {
            return COMMM.DeleteDataById(ID, "MasterMainMenuFacade/MasterDeleteMainMenuDTO/");
        }

    }
}
