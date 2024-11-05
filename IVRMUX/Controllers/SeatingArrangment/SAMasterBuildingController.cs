using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.SeatingArrangment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.SeatingArrangment;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.SeatingArrangment
{
    [Route("api/[controller]")]
    public class SAMasterBuildingController : Controller
    {
        SAMasterBuildingDelegate _delg = new SAMasterBuildingDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("LoadData/{id:int}")]
        public SAMasterBuildingDTO LoadData(int id)
        {
            SAMasterBuildingDTO data = new SAMasterBuildingDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId")); 
            return _delg.LoadData(data);
        }

        [Route("SaveMasterBuilding")]
        public SAMasterBuildingDTO SaveMasterBuilding([FromBody] SAMasterBuildingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId")); 
            return _delg.SaveMasterBuilding(data);
        }

        [Route("EditMasterBuilding")]
        public SAMasterBuildingDTO EditMasterBuilding([FromBody] SAMasterBuildingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId")); 
            return _delg.EditMasterBuilding(data);
        }

        [Route("ActiveDeactiveMasterBuilding")]
        public SAMasterBuildingDTO ActiveDeactiveMasterBuilding([FromBody] SAMasterBuildingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId")); 
            return _delg.ActiveDeactiveMasterBuilding(data);
        }

        // Master Floor Data

        [Route("OnFloorDataLoad/{id:int}")]
        public SAMasterBuildingDTO OnFloorDataLoad(int id)
        {
            SAMasterBuildingDTO data = new SAMasterBuildingDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.OnFloorDataLoad(data);
        }

        [Route("SaveMasterFloor")]
        public SAMasterBuildingDTO SaveMasterFloor([FromBody] SAMasterBuildingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.SaveMasterFloor(data);
        }    

        [Route("EditMasterFloor")]
        public SAMasterBuildingDTO EditMasterFloor([FromBody] SAMasterBuildingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.EditMasterFloor(data);
        }

        [Route("ActiveDeactiveMasterFloor")]
        public SAMasterBuildingDTO ActiveDeactiveMasterFloor([FromBody] SAMasterBuildingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.ActiveDeactiveMasterFloor(data);
        }

        // Master Room

        [Route("OnRoomDataLoad/{id:int}")]
        public SAMasterBuildingDTO OnRoomDataLoad(int id)
        {
            SAMasterBuildingDTO data = new SAMasterBuildingDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.OnRoomDataLoad(data);
        }

        [Route("OnChangeBuilding")]
        public SAMasterBuildingDTO OnChangeBuilding([FromBody] SAMasterBuildingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.OnChangeBuilding(data);
        }

        [Route("SaveMasterRoom")]
        public SAMasterBuildingDTO SaveMasterRoom([FromBody] SAMasterBuildingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.SaveMasterRoom(data);
        }

        [Route("EditMasterRoom")]
        public SAMasterBuildingDTO EditMasterRoom([FromBody] SAMasterBuildingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.EditMasterRoom(data);
        }

        [Route("ActiveDeactiveMasterRoom")]
        public SAMasterBuildingDTO ActiveDeactiveMasterRoom([FromBody] SAMasterBuildingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.ActiveDeactiveMasterRoom(data);
        }

        // Master University Exam
        [Route("OnUniversityExamLoadData/{id:int}")]
        public SAMasterBuildingDTO OnUniversityExamLoadData(int id)
        {
            SAMasterBuildingDTO data = new SAMasterBuildingDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.OnUniversityExamLoadData(data);
        }

        [Route("SaveMasterUniversityExam")]
        public SAMasterBuildingDTO SaveMasterUniversityExam([FromBody] SAMasterBuildingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.SaveMasterUniversityExam(data);
        }

        [Route("EditMasterUniverstityExam")]
        public SAMasterBuildingDTO EditMasterUniverstityExam([FromBody] SAMasterBuildingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.EditMasterUniverstityExam(data);
        }

        [Route("ActiveDeactiveMasterUniverstityExam")]
        public SAMasterBuildingDTO ActiveDeactiveMasterUniverstityExam([FromBody] SAMasterBuildingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.ActiveDeactiveMasterUniverstityExam(data);
        }

        [Route("UpdateMasterUniversityExamOrder")]
        public SAMasterBuildingDTO UpdateMasterUniversityExamOrder([FromBody] SAMasterBuildingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.UpdateMasterUniversityExamOrder(data);
        }

        // Master  Duty Type
        [Route("OnDutyTypeLoadData/{id:int}")]
        public SAMasterBuildingDTO OnDutyTypeLoadData(int id)
        {
            SAMasterBuildingDTO data = new SAMasterBuildingDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.OnDutyTypeLoadData(data);
        }

        [Route("SaveMasterDutyType")]
        public SAMasterBuildingDTO SaveMasterDutyType([FromBody] SAMasterBuildingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.SaveMasterDutyType(data);
        }

        [Route("EditDutyType")]
        public SAMasterBuildingDTO EditDutyType([FromBody] SAMasterBuildingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.EditDutyType(data);
        }

        [Route("ActiveDeactiveDutyType")]
        public SAMasterBuildingDTO ActiveDeactiveDutyType([FromBody] SAMasterBuildingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.ActiveDeactiveDutyType(data);
        }

        // Master Time Slot
        [Route("OnTimeSlotLoadData/{id:int}")]
        public SAMasterBuildingDTO OnTimeSlotLoadData(int id)
        {
            SAMasterBuildingDTO data = new SAMasterBuildingDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.OnTimeSlotLoadData(data);
        }

        [Route("SaveMasterTimeSlot")]
        public SAMasterBuildingDTO SaveMasterTimeSlot([FromBody] SAMasterBuildingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.SaveMasterTimeSlot(data);
        }

        [Route("EditTimeSlot")]
        public SAMasterBuildingDTO EditTimeSlot([FromBody] SAMasterBuildingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.EditTimeSlot(data);
        }

        [Route("ActiveDeactiveTimeSlot")]
        public SAMasterBuildingDTO ActiveDeactiveTimeSlot([FromBody] SAMasterBuildingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.RoleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.ActiveDeactiveTimeSlot(data);
        }
    }
}
