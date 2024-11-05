using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.HOD.Interfaces
{
  public  interface StudentHODInterface
    {
        ExamDTO getloaddata(ExamDTO data);
        ExamDTO getexamdata(ExamDTO data);
        ExamDTO getexamdetails(ExamDTO sddto);
        ExamDTO getsectiondata(ExamDTO sddto);
        ExamDTO get_classes(ExamDTO sddto);
        ExamDTO getstudentdata(ExamDTO sddto);
    }
}
