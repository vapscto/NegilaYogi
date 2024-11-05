using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model.com.vaps.admission;
using PreadmissionDTOs.com.vaps.admission;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using DomainModel.Model;
using PreadmissionDTOs;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class AdmissionRegisterController : Controller
    {

        AdmissionRegisterDelegates AdmissionRegisterdelStr = new AdmissionRegisterDelegates();

      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails/")]
        public SchoolYearWiseStudentDTO Getdetails(SchoolYearWiseStudentDTO SchoolYearWiseStudentDTO)
        {
            SchoolYearWiseStudentDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return AdmissionRegisterdelStr.GetAdmissionRegisterData(SchoolYearWiseStudentDTO);            
        }
        [HttpPost]
        [Route("Getdetailsreport/")]
        public SchoolYearWiseStudentDTO Getdetailsreport([FromBody] SchoolYearWiseStudentDTO MMD)
        {
            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return AdmissionRegisterdelStr.Getdetailsreport(MMD);
        }


        [Route("getclass")]
        public SchoolYearWiseStudentDTO getclass([FromBody] SchoolYearWiseStudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return AdmissionRegisterdelStr.getclass(data);
        }

        [Route("GetSelectedRowdetails/{id:int}")]
        public castecategoryDTO GetSelectedRowDetails(int ID)
        {
            HttpContext.Session.SetString("castecategoryID", ID.ToString());
            return AdmissionRegisterdelStr.GetSelectedRowDetails(ID);
        }


        [HttpPost]      
        public castecategoryDTO castecategoryDTO([FromBody] castecategoryDTO MMD)
        {
            Int32 castecategoryID = 0;
            if (HttpContext.Session.GetString("castecategoryID") != null)
            {
                castecategoryID = Convert.ToInt32(HttpContext.Session.GetString("castecategoryID"));
            }
            MMD.IMCC_Id = castecategoryID;
            HttpContext.Session.Remove("castecategoryID");
            AdmissionRegisterdelStr.castecategoryData(MMD);         
            return MMD;           
        }

        [HttpDelete]
        [Route("MasterDeleteModulesDTO/{id:int}")]
        public castecategoryDTO castecategoryDTO(int ID)
        {
            return AdmissionRegisterdelStr.MasterDeleteModulesData(ID);         
        }
    }

}
