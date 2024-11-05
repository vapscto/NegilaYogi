using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;
using CommonLibrary;
using System.Net.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Delegates.com.vapstech.Sports
{
    [Route("api/[controller]")]
    public class SportStudentHouseDivisionDelegate : Controller
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<SportMasterHouseDTO, SportMasterHouseDTO> COMMM = new CommonDelegate<SportMasterHouseDTO, SportMasterHouseDTO>();
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
                HttpResponseMessage response = client.GetAsync("api/SportStudentHouseDivisionFacade/" + resource).Result;
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

        public SportMasterHouseDTO GetmastercasteData(SportMasterHouseDTO lo)
        {
            return COMMM.POSTDataSports(lo, "SportStudentHouseDivisionFacade/Getdetails");
        }
        public SportMasterHouseDTO get_section(SportMasterHouseDTO lo)
        {
            return COMMM.POSTDataSports(lo, "SportStudentHouseDivisionFacade/get_section");
        }
        public SportMasterHouseDTO get_student(SportMasterHouseDTO lo)
        {
            return COMMM.POSTDataSports(lo, "SportStudentHouseDivisionFacade/get_student");
        }

        public SportMasterHouseDTO GetSelectedRowDetails(SportMasterHouseDTO lo)//Int32 AMA_Id
        {
            return COMMM.POSTDataSports(lo, "SportStudentHouseDivisionFacade/GetSelectedRowDetails/");
        }

        public SportMasterHouseDTO mastercasteData(SportMasterHouseDTO SportMasterHouseDTO)//Int32 IVRMM_Id
        {
            return COMMM.POSTDataSports(SportMasterHouseDTO, "SportStudentHouseDivisionFacade/");
        }

        public SportMasterHouseDTO deactivate(SportMasterHouseDTO rel)
        {
            return COMMM.POSTDataSports(rel, "SportStudentHouseDivisionFacade/deactivate/");

        }
    }
}
