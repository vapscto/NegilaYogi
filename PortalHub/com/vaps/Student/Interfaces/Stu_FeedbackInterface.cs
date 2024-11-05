using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Portals.Student;

namespace PortalHub.com.vaps.Student.Interfaces
{
    public interface Stu_FeedbackInterface
    {
        Stu_FeedbackDTO getloaddata(Stu_FeedbackDTO data);
        Stu_FeedbackDTO savecomment(Stu_FeedbackDTO data);
        Stu_FeedbackDTO getexamdetails(Stu_FeedbackDTO sddto);
    }
}
