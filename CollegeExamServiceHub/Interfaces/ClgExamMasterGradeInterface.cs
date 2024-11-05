using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.Interfaces
{
    public interface ClgExamMasterGradeInterface
    {
        MasterExamGradeDTO savedetail(MasterExamGradeDTO data);
        MasterExamGradeDTO deactivate(MasterExamGradeDTO data);
        MasterExamGradeDTO getdetails(int id);
        MasterExamGradeDTO getpageedit(int id);
        MasterExamGradeDTO getalldetailsviewrecords(int id);
        MasterExamGradeDTO deleterec(int id);

    }
}
