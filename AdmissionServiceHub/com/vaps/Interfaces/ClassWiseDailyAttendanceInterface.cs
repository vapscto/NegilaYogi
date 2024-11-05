using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;
using PreadmissionDTOs;
using DomainModel.Model.com.vaps.admission;

//PreadmissionDTOs.com.vaps.admission


namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface ClassWiseDailyAttendanceInterface
    {
        //castecategoryDTO castecategoryData(castecategoryDTO mas);

        //castecategoryDTO MasterDeleteModulesData(int ID);

        //castecategoryDTO GetSelectedRowDetails(int ID);

        SchoolYearWiseStudentDTO GetddlDatabind(SchoolYearWiseStudentDTO clswisedailyattDTO);
        SchoolYearWiseStudentDTO getsection(SchoolYearWiseStudentDTO clswisedailyattDTO);
        SchoolYearWiseStudentDTO setfromdate(SchoolYearWiseStudentDTO clswisedailyattDTO);
        Task<SchoolYearWiseStudentDTO> absentsendsms(SchoolYearWiseStudentDTO data);
    }
}
