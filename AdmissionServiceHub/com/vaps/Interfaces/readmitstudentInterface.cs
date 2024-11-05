using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
   public interface readmitstudentInterface
    {
        SchoolYearWiseStudentDTO GetDropDownList(SchoolYearWiseStudentDTO scAllt);

        SchoolYearWiseStudentDTO GetStudentListByYear(long id);

        readmitstudentDTO savereadmit_student(readmitstudentDTO scAllt);
        SchoolYearWiseStudentDTO GetstudentdetailsbyYearandclass(SchoolYearWiseStudentDTO scAllt);
        SchoolYearWiseStudentDTO getnewjoinlist(SchoolYearWiseStudentDTO data);

    }
}
