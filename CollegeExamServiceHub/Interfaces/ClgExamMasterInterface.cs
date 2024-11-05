using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.Interfaces
{
    public interface ClgExamMasterInterface
    {
        exammasterDTO Getdetails(exammasterDTO data);
        exammasterDTO savedetails(exammasterDTO data);
        exammasterDTO editdetails(int data);
        exammasterDTO validateordernumber(exammasterDTO data);
        exammasterDTO deactivate(exammasterDTO data);
    }
}
