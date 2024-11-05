using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model;
using PreadmissionDTOs;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
   // [EnableCors("AllowSpecificOrigin")]

    public class MasterHeaderDetailsController : Controller
    {
        // GET: /<controller>/
        MasterHeaderDetailsDelegates MasterHeaderDetailsStr = new MasterHeaderDetailsDelegates();

        //// GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [Route("Getdetails/")]
        public MasterHeaderDetailsDTO Getdetails(MasterHeaderDetailsDTO MasterHeaderDetailsDTO)
        {
            MasterHeaderDetailsDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return MasterHeaderDetailsStr.GetMasterHeaderDetailsData(MasterHeaderDetailsDTO);
        }

        [Route("GetSelectedRowdetails")]
        public MasterHeaderDetailsDTO GetSelectedRowDetails([FromBody] MasterHeaderDetailsDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return MasterHeaderDetailsStr.GetSelectedRowDetails(dto);

        }

        
        [Route("getmodulePage/{id:int}")]
        public MasterHeaderDetailsDTO getmodulePage(int ID)
        {
            MasterHeaderDetailsDTO data = new MasterHeaderDetailsDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMIM_Id = ID;
            return MasterHeaderDetailsStr.getmodulePage(data);
        }



        [HttpPost]
        [Route("SaveData")]
        public MasterHeaderDetailsDTO SaveData([FromBody] MasterHeaderDetailsDTO MMD)
        {
            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return MasterHeaderDetailsStr.SaveData(MMD);
        }

        [HttpDelete]
        [Route("DeleteEntry/{id:int}")]
        public MasterHeaderDetailsDTO DeleteEntry(int ID)
        {
            return MasterHeaderDetailsStr.DeleteEntry(ID);
        }




    }

}
