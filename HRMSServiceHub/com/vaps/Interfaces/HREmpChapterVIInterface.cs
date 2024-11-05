using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
   public interface HREmpChapterVIInterface
    {

        HR_Emp_ChapterVIDTO getBasicData(HR_Emp_ChapterVIDTO dto);
        HR_Emp_ChapterVIDTO SaveUpdate(HR_Emp_ChapterVIDTO dto);
        HR_Emp_ChapterVIDTO editData(int id);
        HR_Emp_ChapterVIDTO deactivate(HR_Emp_ChapterVIDTO dto);
        HR_Emp_ChapterVIDTO getDetailsByEmployee(HR_Emp_ChapterVIDTO dto);
       
    }
}
