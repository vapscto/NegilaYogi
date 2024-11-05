using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CommonLibrary;
using System.Net.Http;
using PreadmissionDTOs.com.vaps.Sport;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Delegates.com.vapstech.Sport
{
    [Route("api/[controller]")]
    public class SportMasterDivisionDelegate : Controller
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<SportMasterDivisionDTO, SportMasterDivisionDTO> COMMM = new CommonDelegate<SportMasterDivisionDTO, SportMasterDivisionDTO>();
        public string getData(long resource)
        {
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:55229/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

            // HTTP GET
            try
            {
                HttpResponseMessage response = client.GetAsync("api/SportMasterDivisionFacade/" + resource).Result;
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

        public SportMasterDivisionDTO GetmastercasteData(SportMasterDivisionDTO lo)
        {
            return COMMM.POSTDataSports(lo, "SportMasterDivisionFacade/Getdetails");
        }

        public SportMasterDivisionDTO GetSelectedRowDetails(int ID)//Int32 AMA_Id
        {
            return COMMM.GetDataByIdSports(ID, "SportMasterDivisionFacade/GetSelectedRowDetails/");
        }

        public SportMasterDivisionDTO mastercasteData(SportMasterDivisionDTO SportMasterDivisionDTO)//Int32 IVRMM_Id
        {
            return COMMM.POSTDataSports(SportMasterDivisionDTO, "SportMasterDivisionFacade/");           
        }

        public SportMasterDivisionDTO deactivate(SportMasterDivisionDTO rel)
        {
            return COMMM.POSTDataSports(rel, "SportMasterDivisionFacade/deactivate/");

        }

        //public SportMasterDivisionDTO MasterDeleteModulesData(int ID)//Int32 IVRMM_Id
        //{
        //    return COMMM.DeleteSportDataByIdADM(ID, "SportMasterDivisionFacade/MasterDeleteModulesDATA/");
        //}


    }
}
