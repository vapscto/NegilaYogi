using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.SeatingArrangment;

namespace IVRMUX.Delegates.SeatingArrangment
{
    public class SAMasterBuildingDelegate
    {
        CommonDelegate<SAMasterBuildingDTO, SAMasterBuildingDTO> _comm = new CommonDelegate<SAMasterBuildingDTO, SAMasterBuildingDTO>();

        public SAMasterBuildingDTO LoadData(SAMasterBuildingDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "SAMasterBuildingFacade/LoadData");
        }
        public SAMasterBuildingDTO SaveMasterBuilding(SAMasterBuildingDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "SAMasterBuildingFacade/SaveMasterBuilding");
        }
        public SAMasterBuildingDTO EditMasterBuilding(SAMasterBuildingDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "SAMasterBuildingFacade/EditMasterBuilding");
        }
        public SAMasterBuildingDTO ActiveDeactiveMasterBuilding(SAMasterBuildingDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "SAMasterBuildingFacade/ActiveDeactiveMasterBuilding");
        }

        // Master Floor Data
        public SAMasterBuildingDTO OnFloorDataLoad(SAMasterBuildingDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "SAMasterBuildingFacade/OnFloorDataLoad");
        }
        public SAMasterBuildingDTO SaveMasterFloor(SAMasterBuildingDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "SAMasterBuildingFacade/SaveMasterFloor");
        }
        public SAMasterBuildingDTO EditMasterFloor(SAMasterBuildingDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "SAMasterBuildingFacade/EditMasterFloor");
        }
        public SAMasterBuildingDTO ActiveDeactiveMasterFloor(SAMasterBuildingDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "SAMasterBuildingFacade/ActiveDeactiveMasterFloor");
        }

        // Master Room
        public SAMasterBuildingDTO OnRoomDataLoad(SAMasterBuildingDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "SAMasterBuildingFacade/OnRoomDataLoad");
        }
        public SAMasterBuildingDTO OnChangeBuilding(SAMasterBuildingDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "SAMasterBuildingFacade/OnChangeBuilding");
        }
        public SAMasterBuildingDTO SaveMasterRoom(SAMasterBuildingDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "SAMasterBuildingFacade/SaveMasterRoom");
        }
        public SAMasterBuildingDTO EditMasterRoom(SAMasterBuildingDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "SAMasterBuildingFacade/EditMasterRoom");
        }
        public SAMasterBuildingDTO ActiveDeactiveMasterRoom(SAMasterBuildingDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "SAMasterBuildingFacade/ActiveDeactiveMasterRoom");
        }

        // Master University Exam
        public SAMasterBuildingDTO OnUniversityExamLoadData(SAMasterBuildingDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "SAMasterBuildingFacade/OnUniversityExamLoadData");
        }
        public SAMasterBuildingDTO SaveMasterUniversityExam(SAMasterBuildingDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "SAMasterBuildingFacade/SaveMasterUniversityExam");
        }
        public SAMasterBuildingDTO EditMasterUniverstityExam(SAMasterBuildingDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "SAMasterBuildingFacade/EditMasterUniverstityExam");
        }
        public SAMasterBuildingDTO ActiveDeactiveMasterUniverstityExam(SAMasterBuildingDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "SAMasterBuildingFacade/ActiveDeactiveMasterUniverstityExam");
        }
        public SAMasterBuildingDTO UpdateMasterUniversityExamOrder(SAMasterBuildingDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "SAMasterBuildingFacade/UpdateMasterUniversityExamOrder");
        }

        // Master Duty Type
        public SAMasterBuildingDTO OnDutyTypeLoadData(SAMasterBuildingDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "SAMasterBuildingFacade/OnDutyTypeLoadData");
        }
        public SAMasterBuildingDTO SaveMasterDutyType(SAMasterBuildingDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "SAMasterBuildingFacade/SaveMasterDutyType");
        }
        public SAMasterBuildingDTO EditDutyType(SAMasterBuildingDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "SAMasterBuildingFacade/EditDutyType");
        }
        public SAMasterBuildingDTO ActiveDeactiveDutyType(SAMasterBuildingDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "SAMasterBuildingFacade/ActiveDeactiveDutyType");
        }

        // Master Time Slot
        public SAMasterBuildingDTO OnTimeSlotLoadData(SAMasterBuildingDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "SAMasterBuildingFacade/OnTimeSlotLoadData");
        }
        public SAMasterBuildingDTO SaveMasterTimeSlot(SAMasterBuildingDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "SAMasterBuildingFacade/SaveMasterTimeSlot");
        }
        public SAMasterBuildingDTO EditTimeSlot(SAMasterBuildingDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "SAMasterBuildingFacade/EditTimeSlot");
        }
        public SAMasterBuildingDTO ActiveDeactiveTimeSlot(SAMasterBuildingDTO data)
        {
            return _comm.SeatingArrangmentPOST(data, "SAMasterBuildingFacade/ActiveDeactiveTimeSlot");
        }

    }
}
