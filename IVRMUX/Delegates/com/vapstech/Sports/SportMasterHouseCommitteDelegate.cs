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
    public class SportMasterHouseCommitteDelegate : Controller
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HouseCommitte_DTO, HouseCommitte_DTO> COMMM = new CommonDelegate<HouseCommitte_DTO, HouseCommitte_DTO>();
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
                HttpResponseMessage response = client.GetAsync("api/SportMasterHouseCommitteFacade/" + resource).Result;
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

        public HouseCommitte_DTO GetmastercasteData(HouseCommitte_DTO lo)
        {
            return COMMM.POSTDataSports(lo, "SportMasterHouseCommitteFacade/Getdetails");
        }
        public HouseCommitte_DTO get_section(HouseCommitte_DTO lo)
        {
            return COMMM.POSTDataSports(lo, "SportMasterHouseCommitteFacade/get_section");
        }
        public HouseCommitte_DTO get_student(HouseCommitte_DTO lo)
        {
            return COMMM.POSTDataSports(lo, "SportMasterHouseCommitteFacade/get_student");
        }

        public HouseCommitte_DTO GetSelectedRowDetails(HouseCommitte_DTO lo)//Int32 AMA_Id
        {
            return COMMM.POSTDataSports(lo, "SportMasterHouseCommitteFacade/GetSelectedRowDetails/");
        }

        public HouseCommitte_DTO mastercasteData(HouseCommitte_DTO HouseCommitte_DTO)//Int32 IVRMM_Id
        {
            return COMMM.POSTDataSports(HouseCommitte_DTO, "SportMasterHouseCommitteFacade/");
        }
        public HouseCommitte_DTO deactivate(HouseCommitte_DTO rel)
        {
            return COMMM.POSTDataSports(rel, "SportMasterHouseCommitteFacade/deactivate/");

        }
        public HouseCommitte_DTO onhousechage(HouseCommitte_DTO rel)
        {
            return COMMM.POSTDataSports(rel, "SportMasterHouseCommitteFacade/onhousechage/");

        }

        public HouseCommitte_DTO get_House(HouseCommitte_DTO rel)
        {
            return COMMM.POSTDataSports(rel, "SportMasterHouseCommitteFacade/get_House/");

        }
        
    }
}
