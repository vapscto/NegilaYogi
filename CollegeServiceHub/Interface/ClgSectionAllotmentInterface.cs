using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.College.Admission;

namespace CollegeServiceHub.Interface
{
   public  interface ClgSectionAllotmentInterface
    {
        ClgYearWiseStudentDTO GetDropDownList(ClgYearWiseStudentDTO scAllt);

        ClgYearWiseStudentDTO GetStudentListByYear(long id);

        ClgYearWiseStudentDTO saveSctionAllotment(ClgYearWiseStudentDTO scAllt);
        ClgYearWiseStudentDTO GetstudentdetailsbyYearandclass(ClgYearWiseStudentDTO scAllt);
        ClgYearWiseStudentDTO GetStudentListByURN(ClgYearWiseStudentDTO data);
        ClgYearWiseStudentDTO Getbranch(ClgYearWiseStudentDTO data);
        ClgYearWiseStudentDTO GetPromocourse(ClgYearWiseStudentDTO data);
        ClgYearWiseStudentDTO GetPromobranch(ClgYearWiseStudentDTO data);
        ClgYearWiseStudentDTO GetPromosem(ClgYearWiseStudentDTO data);
        ClgYearWiseStudentDTO promsemonchange(ClgYearWiseStudentDTO data);
        ClgYearWiseStudentDTO Get_academiccourse(ClgYearWiseStudentDTO data);
        ClgYearWiseStudentDTO Get_semister(ClgYearWiseStudentDTO data);


        ClgYearWiseStudentDTO GetStudentListByURNsave(ClgYearWiseStudentDTO data);

        // Year Loss section
        ClgYearWiseStudentDTO OnChangeyearlossAcademic(ClgYearWiseStudentDTO data);
        ClgYearWiseStudentDTO Getyearlossbranch(ClgYearWiseStudentDTO data);
        ClgYearWiseStudentDTO Getyearlosssem(ClgYearWiseStudentDTO data);
        ClgYearWiseStudentDTO GetStudentListByYear_yearloss1(ClgYearWiseStudentDTO data);
        

    }
}
