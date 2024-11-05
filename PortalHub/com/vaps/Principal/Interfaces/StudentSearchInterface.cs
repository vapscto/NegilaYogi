using PreadmissionDTOs.com.vaps.Portals.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Principal.Interfaces
{
    public interface StudentSearchInterface
    {
        StudentSearchDTO getdatastuacadgrp(StudentSearchDTO data);
       Task<StudentSearchDTO> getstudentdetails(StudentSearchDTO data);
       Task<StudentSearchDTO> GetStudentDetails1(StudentSearchDTO data);
       Task<StudentSearchDTO> showsectionGrid(StudentSearchDTO data);
        StudentSearchDTO GetStudentSearchByNameOrAdmno(StudentSearchDTO data);
    }
}
