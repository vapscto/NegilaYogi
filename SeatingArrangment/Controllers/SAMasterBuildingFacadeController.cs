using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.SeatingArrangment;
using SeatingArrangment.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SeatingArrangment.Controllers
{
    [Route("api/[controller]")]
    public class SAMasterBuildingFacadeController : Controller
    {
        public SAMasterBuildingInterface _interface;
        public SAMasterBuildingFacadeController(SAMasterBuildingInterface _inter)
        {
            _interface = _inter;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        
        [Route("LoadData")]
        public SAMasterBuildingDTO LoadData([FromBody] SAMasterBuildingDTO data)
        {
            return _interface.LoadData(data);
        }

        [Route("SaveMasterBuilding")]
        public SAMasterBuildingDTO SaveMasterBuilding([FromBody] SAMasterBuildingDTO data)
        {
            return _interface.SaveMasterBuilding(data);
        }

        [Route("EditMasterBuilding")]
        public SAMasterBuildingDTO EditMasterBuilding([FromBody] SAMasterBuildingDTO data)
        {
            return _interface.EditMasterBuilding(data);
        }

        [Route("ActiveDeactiveMasterBuilding")]
        public SAMasterBuildingDTO ActiveDeactiveMasterBuilding([FromBody] SAMasterBuildingDTO data)
        {
            return _interface.ActiveDeactiveMasterBuilding(data);
        }

        //  Master Floor Data

        [Route("OnFloorDataLoad")]
        public SAMasterBuildingDTO OnFloorDataLoad([FromBody] SAMasterBuildingDTO data)
        {
            return _interface.OnFloorDataLoad(data);
        }

        [Route("SaveMasterFloor")]
        public SAMasterBuildingDTO SaveMasterFloor([FromBody] SAMasterBuildingDTO data)
        {
            return _interface.SaveMasterFloor(data);
        }

        [Route("EditMasterFloor")]
        public SAMasterBuildingDTO EditMasterFloor([FromBody] SAMasterBuildingDTO data)
        {
            return _interface.EditMasterFloor(data);
        }

        [Route("ActiveDeactiveMasterFloor")]
        public SAMasterBuildingDTO ActiveDeactiveMasterFloor([FromBody] SAMasterBuildingDTO data)
        {
            return _interface.ActiveDeactiveMasterFloor(data);
        }

        //  Master Room

        [Route("OnRoomDataLoad")]
        public SAMasterBuildingDTO OnRoomDataLoad([FromBody] SAMasterBuildingDTO data)
        {
            return _interface.OnRoomDataLoad(data);
        }

        [Route("OnChangeBuilding")]
        public SAMasterBuildingDTO OnChangeBuilding([FromBody] SAMasterBuildingDTO data)
        {
            return _interface.OnChangeBuilding(data);
        }

        [Route("SaveMasterRoom")]
        public SAMasterBuildingDTO SaveMasterRoom([FromBody] SAMasterBuildingDTO data)
        {
            return _interface.SaveMasterRoom(data);
        }

        [Route("EditMasterRoom")]
        public SAMasterBuildingDTO EditMasterRoom([FromBody] SAMasterBuildingDTO data)
        {
            return _interface.EditMasterRoom(data);
        }

        [Route("ActiveDeactiveMasterRoom")]
        public SAMasterBuildingDTO ActiveDeactiveMasterRoom([FromBody] SAMasterBuildingDTO data)
        {
            return _interface.ActiveDeactiveMasterRoom(data);
        }
        
        //  Master University Exam

        [Route("OnUniversityExamLoadData")]
        public SAMasterBuildingDTO OnUniversityExamLoadData([FromBody] SAMasterBuildingDTO data)
        {
            return _interface.OnUniversityExamLoadData(data);
        }

        [Route("SaveMasterUniversityExam")]
        public SAMasterBuildingDTO SaveMasterUniversityExam([FromBody] SAMasterBuildingDTO data)
        {
            return _interface.SaveMasterUniversityExam(data);
        }

        [Route("EditMasterUniverstityExam")]
        public SAMasterBuildingDTO EditMasterUniverstityExam([FromBody] SAMasterBuildingDTO data)
        {
            return _interface.EditMasterUniverstityExam(data);
        }

        [Route("ActiveDeactiveMasterUniverstityExam")]
        public SAMasterBuildingDTO ActiveDeactiveMasterUniverstityExam([FromBody] SAMasterBuildingDTO data)
        {
            return _interface.ActiveDeactiveMasterUniverstityExam(data);
        }

        [Route("UpdateMasterUniversityExamOrder")]
        public SAMasterBuildingDTO UpdateMasterUniversityExamOrder([FromBody] SAMasterBuildingDTO data)
        {
            return _interface.UpdateMasterUniversityExamOrder(data);
        }

        //  Master Duty Type

        [Route("OnDutyTypeLoadData")]
        public SAMasterBuildingDTO OnDutyTypeLoadData([FromBody] SAMasterBuildingDTO data)
        {
            return _interface.OnDutyTypeLoadData(data);
        }

        [Route("SaveMasterDutyType")]
        public SAMasterBuildingDTO SaveMasterDutyType([FromBody] SAMasterBuildingDTO data)
        {
            return _interface.SaveMasterDutyType(data);
        }

        [Route("EditDutyType")]
        public SAMasterBuildingDTO EditDutyType([FromBody] SAMasterBuildingDTO data)
        {
            return _interface.EditDutyType(data);
        }

        [Route("ActiveDeactiveDutyType")]
        public SAMasterBuildingDTO ActiveDeactiveDutyType([FromBody] SAMasterBuildingDTO data)
        {
            return _interface.ActiveDeactiveDutyType(data);
        }

        //  Master Time Slot

        [Route("OnTimeSlotLoadData")]
        public SAMasterBuildingDTO OnTimeSlotLoadData([FromBody] SAMasterBuildingDTO data)
        {
            return _interface.OnTimeSlotLoadData(data);
        }

        [Route("SaveMasterTimeSlot")]
        public SAMasterBuildingDTO SaveMasterTimeSlot([FromBody] SAMasterBuildingDTO data)
        {
            return _interface.SaveMasterTimeSlot(data);
        }

        [Route("EditTimeSlot")]
        public SAMasterBuildingDTO EditTimeSlot([FromBody] SAMasterBuildingDTO data)
        {
            return _interface.EditTimeSlot(data);
        }

        [Route("ActiveDeactiveTimeSlot")]
        public SAMasterBuildingDTO ActiveDeactiveTimeSlot([FromBody] SAMasterBuildingDTO data)
        {
            return _interface.ActiveDeactiveTimeSlot(data);
        }

    }
}
