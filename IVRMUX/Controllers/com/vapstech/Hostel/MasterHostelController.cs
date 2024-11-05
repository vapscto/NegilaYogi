using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IVRMUX.Delegates.com.vapstech.Hostel;
using PreadmissionDTOs.com.vaps.Hostel;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Hostel
{
    [Route("api/[controller]")]
    public class MasterHostelController : Controller
    {
        public MasterHostelDelegate _delObj = new MasterHostelDelegate();

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

       [Route("Page_loaddata/{id:int}")]
        public HL_Master_Hostel_DTO Page_loaddata(int id)
        {
            HL_Master_Hostel_DTO data = new HL_Master_Hostel_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           // data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delObj.Page_loaddata(data);
        }

        [Route("Get_StateData")]
        public HL_Master_Hostel_DTO Get_StateData([FromBody] HL_Master_Hostel_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delObj.Get_StateData(data);
        }

        [Route("Save_Hostel_Data")]
        public HL_Master_Hostel_DTO Save_Hostel_Data([FromBody] HL_Master_Hostel_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delObj.Save_Hostel_Data(data);
        }

        [Route("Edit_Hostel_Row")]
        public HL_Master_Hostel_DTO Edit_Hostel_Row([FromBody]HL_Master_Hostel_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delObj.Edit_Hostel_Row(data);
        }

        [Route("Deactive_Hostel_Row")]
        public HL_Master_Hostel_DTO Deactive_Hostel_Row([FromBody]HL_Master_Hostel_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delObj.Deactive_Hostel_Row(data);
        }
        [Route("Get_MappedFacility")]
        public HL_Master_Hostel_DTO Get_MappedFacility([FromBody]HL_Master_Hostel_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delObj.Get_MappedFacility(data);
        }
        [Route("Get_MappedEmpl")]
        public HL_Master_Hostel_DTO Get_MappedEmpl([FromBody]HL_Master_Hostel_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delObj.Get_MappedEmpl(data);
        }

        [Route("viewuploadflies")]
        public HL_Master_Hostel_DTO viewuploadflies([FromBody] HL_Master_Hostel_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delObj.viewuploadflies(data);
        }

        [Route("deleteuploadfile")]
        public HL_Master_Hostel_DTO deleteuploadfile([FromBody] HL_Master_Hostel_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delObj.deleteuploadfile(data);
        }

    }
}
