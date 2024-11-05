using PreadmissionDTOs.com.vaps.College;
using PreadmissionDTOs.com.vaps.College.Exam;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.Interfaces
{
    public interface ClgMasterSubjectGroupInterface
    {
        MasterSubjectGroupDTO savedetail(MasterSubjectGroupDTO data);
        MasterSubjectGroupDTO deactivate(MasterSubjectGroupDTO data);
        MasterSubjectGroupDTO getdetails(int id);
        MasterSubjectGroupDTO getpageedit(int id);
        Exm_Col_Master_Group_SubjectsDTO getalldetailsviewrecords(int id);
        MasterSubjectGroupDTO deleterec(int id);
    }
}
