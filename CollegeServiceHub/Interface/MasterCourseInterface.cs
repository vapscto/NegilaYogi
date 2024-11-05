using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
     public  interface MasterCourseInterface
    {
        MasterCourseDTO Savedetails(MasterCourseDTO id);
        MasterCourseDTO getalldetails(int id);
        MasterCourseDTO Deletedetails(MasterCourseDTO id);
        MasterCourseDTO getOrder(MasterCourseDTO id);
        MasterCourseDTO EditData(MasterCourseDTO id);
        
    }
}
