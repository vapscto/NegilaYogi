using PreadmissionDTOs.SeatingArrangment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatingArrangment.Interface
{
    public interface SAMasterBuildingInterface
    {
        SAMasterBuildingDTO LoadData(SAMasterBuildingDTO data);
        SAMasterBuildingDTO SaveMasterBuilding(SAMasterBuildingDTO data);
        SAMasterBuildingDTO EditMasterBuilding(SAMasterBuildingDTO data);
        SAMasterBuildingDTO ActiveDeactiveMasterBuilding(SAMasterBuildingDTO data);

        // Master Floor Data
        SAMasterBuildingDTO OnFloorDataLoad(SAMasterBuildingDTO data);
        SAMasterBuildingDTO SaveMasterFloor(SAMasterBuildingDTO data);
        SAMasterBuildingDTO EditMasterFloor(SAMasterBuildingDTO data);
        SAMasterBuildingDTO ActiveDeactiveMasterFloor(SAMasterBuildingDTO data);

        // Master Room Data
        SAMasterBuildingDTO OnRoomDataLoad(SAMasterBuildingDTO data);
        SAMasterBuildingDTO OnChangeBuilding(SAMasterBuildingDTO data);
        SAMasterBuildingDTO SaveMasterRoom(SAMasterBuildingDTO data);
        SAMasterBuildingDTO EditMasterRoom(SAMasterBuildingDTO data);
        SAMasterBuildingDTO ActiveDeactiveMasterRoom(SAMasterBuildingDTO data);

        // Master University Exam
        SAMasterBuildingDTO OnUniversityExamLoadData(SAMasterBuildingDTO data);
        SAMasterBuildingDTO SaveMasterUniversityExam(SAMasterBuildingDTO data);
        SAMasterBuildingDTO EditMasterUniverstityExam(SAMasterBuildingDTO data);
        SAMasterBuildingDTO ActiveDeactiveMasterUniverstityExam(SAMasterBuildingDTO data);
        SAMasterBuildingDTO UpdateMasterUniversityExamOrder(SAMasterBuildingDTO data);

        // Master Duty Type
        SAMasterBuildingDTO OnDutyTypeLoadData(SAMasterBuildingDTO data);
        SAMasterBuildingDTO SaveMasterDutyType(SAMasterBuildingDTO data);
        SAMasterBuildingDTO EditDutyType(SAMasterBuildingDTO data);
        SAMasterBuildingDTO ActiveDeactiveDutyType(SAMasterBuildingDTO data);

        // Master Time Slot
        SAMasterBuildingDTO OnTimeSlotLoadData(SAMasterBuildingDTO data);
        SAMasterBuildingDTO SaveMasterTimeSlot(SAMasterBuildingDTO data);
        SAMasterBuildingDTO EditTimeSlot(SAMasterBuildingDTO data);
        SAMasterBuildingDTO ActiveDeactiveTimeSlot(SAMasterBuildingDTO data);
    }
}
