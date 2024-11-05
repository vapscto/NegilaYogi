using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface CollegeActiveDeactiveStudentsInterface
    {
        CollegeActiveDeactiveStudentsDTO getdata(CollegeActiveDeactiveStudentsDTO data);
        CollegeActiveDeactiveStudentsDTO onacademicyearchange(CollegeActiveDeactiveStudentsDTO data);
        CollegeActiveDeactiveStudentsDTO oncoursechange(CollegeActiveDeactiveStudentsDTO data);
        CollegeActiveDeactiveStudentsDTO onbranchchange(CollegeActiveDeactiveStudentsDTO data);
        CollegeActiveDeactiveStudentsDTO onchangesemester(CollegeActiveDeactiveStudentsDTO data);
        CollegeActiveDeactiveStudentsDTO search(CollegeActiveDeactiveStudentsDTO data);
        CollegeActiveDeactiveStudentsDTO savedata(CollegeActiveDeactiveStudentsDTO data);
        CollegeActiveDeactiveStudentsDTO getreport(CollegeActiveDeactiveStudentsDTO data);
        
    }
}
