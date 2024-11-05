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
    public class MasterDocumentMappingController : Controller
    {
        // GET: /<controller>/
        MasterDocumentMappingDelegates MasterDocumentMappingStr = new MasterDocumentMappingDelegates();

        //// GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [Route("Getdetails/")]
        public MasterDocumentMappingDTO Getdetails(MasterDocumentMappingDTO MasterDocumentMappingDTO)
        {
            //int drpdata = en.countryid;
            //EnqDTO enq=new EnqDTO();
            return MasterDocumentMappingStr.Getdetails(MasterDocumentMappingDTO);
            
        }

        [Route("GetSelectedRowdetails/{id:int}")]
        public MasterDocumentMappingDTO GetSelectedRowDetails(int ID)
        {
            HttpContext.Session.SetString("DocMapId", ID.ToString()); 

            return MasterDocumentMappingStr.GetSelectedRowDetails(ID);

        }


        [HttpPost]
        public MasterDocumentMappingDTO SaveData([FromBody] MasterDocumentMappingDTO MMD)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.MI_Id = mid;
            int DocMapId = 0;
            if (HttpContext.Session.GetString("DocMapId") != null)
            {
                DocMapId = Convert.ToInt32(HttpContext.Session.GetString("DocMapId"));//Get
                MMD.PASCD_Id = DocMapId;
                HttpContext.Session.Remove("DocMapId");
            }

            return MasterDocumentMappingStr.SaveData(MMD);         
           
        }


        [HttpDelete]
        [Route("DeleteData/{id:int}")]
        public MasterDocumentMappingDTO DeleteData(int ID)
        {
            return MasterDocumentMappingStr.DeleteData(ID);
            //reg.status = "sucess";
            
            // return reg;
        }




    }

}
