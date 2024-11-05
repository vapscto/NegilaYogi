using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface ClgMasterAcademicYearInterface
    {
        ClgMasterAcademicYearDTO getalldetails(ClgMasterAcademicYearDTO data);
        ClgMasterAcademicYearDTO saveaccyear(ClgMasterAcademicYearDTO data);
        ClgMasterAcademicYearDTO edit(ClgMasterAcademicYearDTO data);
        ClgMasterAcademicYearDTO deactivate(ClgMasterAcademicYearDTO data);
        ClgMasterAcademicYearDTO saveorder(ClgMasterAcademicYearDTO data);
        
    }
}
