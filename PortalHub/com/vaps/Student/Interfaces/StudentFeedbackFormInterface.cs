﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Portals.Student;

namespace PortalHub.com.vaps.Student.Interfaces
{
    public interface StudentFeedbackFormInterface
    {
        StudentFeedbackFormDTO getloaddata(StudentFeedbackFormDTO data);
        StudentFeedbackFormDTO savefeedback(StudentFeedbackFormDTO data);
        StudentFeedbackFormDTO deactive(StudentFeedbackFormDTO data);
        
    }
}
