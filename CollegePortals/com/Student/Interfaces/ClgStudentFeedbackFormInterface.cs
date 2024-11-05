using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.College.Portals;

namespace CollegePortals.com.Student.Interfaces
{
    public interface ClgStudentFeedbackFormInterface
    {
        ClgStudentFeedbackFormDTO getloaddata(ClgStudentFeedbackFormDTO data);
        ClgStudentFeedbackFormDTO savefeedback(ClgStudentFeedbackFormDTO data);
        ClgStudentFeedbackFormDTO deactive(ClgStudentFeedbackFormDTO data);
    }
}
