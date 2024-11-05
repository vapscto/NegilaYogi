using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface ClgMasterCourseCategoryMapInterface
    {
        ClgMasterCourseCategoryMapDTO Savedetails(ClgMasterCourseCategoryMapDTO id);
        ClgMasterCourseCategoryMapDTO getalldetails(ClgMasterCourseCategoryMapDTO id);
        ClgMasterCourseCategoryMapDTO deactive(ClgMasterCourseCategoryMapDTO id);
    }
}
