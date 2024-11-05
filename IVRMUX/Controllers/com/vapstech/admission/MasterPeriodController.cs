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

    public class MasterPeriodController : Controller
    {
        // GET: /<controller>/
        MasterPeriodDelegates MasterPeriodStr = new MasterPeriodDelegates();

        //// GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [Route("Getdetails/")]
        public MasterPeriodDTO Getdetails(MasterPeriodDTO MasterPeriodDTO)
        {
            MasterPeriodDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return MasterPeriodStr.GetMasterPeriodData(MasterPeriodDTO);
        }

        [Route("GetSelectedRowdetails")]
        public MasterPeriodDTO GetSelectedRowDetails([FromBody] MasterPeriodDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return MasterPeriodStr.GetSelectedRowDetails(dto);

        }

        [HttpPost]
        public MasterPeriodDTO AttendanceTypeData([FromBody] MasterPeriodDTO MMD)
        {
            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return MasterPeriodStr.SaveData(MMD);
        }

        [HttpGet]
        [Route("DeleteEntry/{id:int}")]
        public MasterPeriodDTO DeleteEntry(int ID)
        {
            return MasterPeriodStr.DeleteEntry(ID);
        }

    }

}
