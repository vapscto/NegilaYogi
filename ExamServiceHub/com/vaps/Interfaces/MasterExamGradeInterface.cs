
using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface MasterExamGradeInterface
    {
        MasterExamGradeDTO savedetail(MasterExamGradeDTO objcategory);
        MasterExamGradeDTO deactivate(MasterExamGradeDTO data);
        MasterExamGradeDTO getdetails(int id);
        MasterExamGradeDTO getpageedit(int id);
        MasterExamGradeDTO getalldetailsviewrecords(int id);
        MasterExamGradeDTO deleterec(int id);


    }
}
