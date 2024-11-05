using PreadmissionDTOs.com.vaps.Portals.Principal;
using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.HOD.Interfaces
{
    public interface HODStudentSearchInterface
    {
        StudentSearchDTO getdatastuacadgrp(StudentSearchDTO data);
        Task<StudentSearchDTO> getstudentdetails(StudentSearchDTO data);
     
    }
}
