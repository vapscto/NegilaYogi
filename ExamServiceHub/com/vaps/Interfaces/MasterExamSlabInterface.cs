
using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface MasterExamSlabInterface
    {
        MasterExamSlabDTO savedetail(MasterExamSlabDTO objcategory);
     
        MasterExamSlabDTO getdetails(int id);
    


    }
}
