using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;
using System.Net.Http;
using CommonLibrary;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Delegates.com.vapstech.Sports
{
    [Route("api/[controller]")]
    public class SportMasterUOMDelegate : Controller
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<SportMasterUOMDTO, SportMasterUOMDTO> COMMM = new CommonDelegate<SportMasterUOMDTO, SportMasterUOMDTO>();
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
                HttpResponseMessage response = client.GetAsync("api/SportMasterUOMFacade/" + resource).Result;
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

        public SportMasterUOMDTO GetmastercasteData(SportMasterUOMDTO lo)
        {
            return COMMM.POSTDataSports(lo, "SportMasterUOMFacade/Getdetails");
        }

        public SportMasterUOMDTO GetSelectedRowDetails(int ID)//Int32 AMA_Id
        {
            return COMMM.GetDataByIdSports(ID, "SportMasterUOMFacade/GetSelectedRowDetails/");
        }

        public SportMasterUOMDTO mastercasteData(SportMasterUOMDTO SportMasterUOMDTO)//Int32 IVRMM_Id
        {
            return COMMM.POSTDataSports(SportMasterUOMDTO, "SportMasterUOMFacade/");
        }

        public SportMasterUOMDTO deactivate(SportMasterUOMDTO rel)
        {
            return COMMM.POSTDataSports(rel, "SportMasterUOMFacade/deactivate/");

        }
    }
}
