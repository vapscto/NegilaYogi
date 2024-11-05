using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.College.Student;
using PreadmissionDTOs.com.vaps.Portals.Student;

namespace CollegePortals.com.Student.Interfaces
{
    public interface CollegeStudent_TTInterface
    {
        CollegeStudent_TTDTO getloaddata(CollegeStudent_TTDTO data);
        Task<CollegeStudent_TTDTO> getStudentTT(CollegeStudent_TTDTO sddto);
    }
}
