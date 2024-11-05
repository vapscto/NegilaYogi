using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.admission;
using PreadmissionDTOs.com.vaps.admission;
using corewebapi18072016.com.vaps.admission.Delegates;

namespace corewebapi18072016.Controllers.com.vapstech.admission
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class MasterDocumentController : Controller
    {
        // GET: /<controller>/
        MasterDocumentDelegates MasterDocumentStr = new MasterDocumentDelegates();

        //// GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [Route("Getdetails/")]
        public MasterDocumentDTO Getdetails(MasterDocumentDTO MasterDocumentDTO)
        {
            MasterDocumentDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return MasterDocumentStr.Getdetails(MasterDocumentDTO);
            
        }

        [Route("GetSelectedRowdetails/{id:int}")]
        public MasterDocumentDTO GetSelectedRowDetails(int ID)
        {
            return MasterDocumentStr.GetSelectedRowDetails(ID);
        }


        [HttpPost]
        public MasterDocumentDTO SaveData([FromBody] MasterDocumentDTO MMD)
        {
            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return MasterDocumentStr.SaveData(MMD);         
           
        }


        [HttpGet]
        [Route("DeleteData/{id:int}")]
        public MasterDocumentDTO DeleteData(int ID)
        {
            return MasterDocumentStr.DeleteData(ID);
            //reg.status = "sucess";
            
            // return reg;
        }




    }

}
