using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
   public interface SectionAllotmentInterface
    {
        SchoolYearWiseStudentDTO GetDropDownList(SchoolYearWiseStudentDTO scAllt);

        SchoolYearWiseStudentDTO GetStudentListByYear(long id);

        SchoolYearWiseStudentDTO saveSctionAllotment(SchoolYearWiseStudentDTO scAllt);
        SchoolYearWiseStudentDTO GetstudentdetailsbyYearandclass(SchoolYearWiseStudentDTO scAllt);
        SchoolYearWiseStudentDTO GetStudentListByURN(SchoolYearWiseStudentDTO data);
        Student_Update_RollNumber GetStudentListByURNsave(Student_Update_RollNumber data);

        //Change Class
        SchoolYearWiseStudentDTO GetChangeClassDetails(SchoolYearWiseStudentDTO scAllt);
        SchoolYearWiseStudentDTO GetStudentListByYearCLS(SchoolYearWiseStudentDTO data);
        SchoolYearWiseStudentDTO onstudentnamechange(SchoolYearWiseStudentDTO data);
        SchoolYearWiseStudentDTO DeleteFeeMapping(SchoolYearWiseStudentDTO data);
        SchoolYearWiseStudentDTO SaveClassChange(SchoolYearWiseStudentDTO data);
        SchoolYearWiseStudentDTO SaveClassFeeChange(SchoolYearWiseStudentDTO data);
    }
}
