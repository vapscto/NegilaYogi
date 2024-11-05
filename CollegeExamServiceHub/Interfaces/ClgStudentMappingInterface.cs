using PreadmissionDTOs.com.vaps.College.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.Interfaces
{
  public  interface ClgStudentMappingInterface
    {
        Exm_Col_Studentwise_SubjectsDTO getdetails(int objcategory);
        Exm_Col_Studentwise_SubjectsDTO Studentdetails(Exm_Col_Studentwise_SubjectsDTO data);
        Exm_Col_Studentwise_SubjectsDTO savedetails(Exm_Col_Studentwise_SubjectsDTO data);
        Exm_Col_Studentwise_SubjectsDTO getcourse(Exm_Col_Studentwise_SubjectsDTO data);
        Exm_Col_Studentwise_SubjectsDTO getbranch(Exm_Col_Studentwise_SubjectsDTO data);
        Exm_Col_Studentwise_SubjectsDTO getsemester(Exm_Col_Studentwise_SubjectsDTO data);
        Exm_Col_Studentwise_SubjectsDTO getsection(Exm_Col_Studentwise_SubjectsDTO data);
    }
}
