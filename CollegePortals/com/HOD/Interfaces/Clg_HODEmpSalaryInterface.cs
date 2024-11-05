using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.College.Portals;

namespace CollegePortals.com.Student.Interfaces
{
    public interface Clg_HODEmpSalaryInterface
    {
        Task<Clg_HODEmpSalaryDTO> Getdetails(Clg_HODEmpSalaryDTO data);
     


    }

}
