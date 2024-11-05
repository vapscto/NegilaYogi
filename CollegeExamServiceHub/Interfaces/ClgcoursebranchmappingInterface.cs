using PreadmissionDTOs.com.vaps.College.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.Interfaces
{
  public  interface ClgcoursebranchmappingInterface
    {
        Exm_Col_CourseBranchDTO getdetails(int objcategory);
        Exm_Col_CourseBranchDTO savedetail2(Exm_Col_CourseBranchDTO objcategory);
        Exm_Col_CourseBranchDTO getbranch(Exm_Col_CourseBranchDTO objcategory);        
        Exm_Col_CourseBranchDTO get_subjects(Exm_Col_CourseBranchDTO objcategory);
        Exm_Col_CourseBranchDTO getalldetailsviewrecords(Exm_Col_CourseBranchDTO objcategory);
        Exm_Col_CourseBranchDTO deactivate(Exm_Col_CourseBranchDTO data);
        Exm_Col_CourseBranchDTO editdeatils(int objcategory);
    }
}
