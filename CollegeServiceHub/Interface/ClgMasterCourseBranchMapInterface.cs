using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace CollegeServiceHub.Interface
{
    public interface ClgMasterCourseBranchMapInterface
    {
        ClgMasterCourseBranchMapDTO Savedetails(ClgMasterCourseBranchMapDTO id);
        ClgMasterCourseBranchMapDTO getalldetails(ClgMasterCourseBranchMapDTO id);
        ClgMasterCourseBranchMapDTO Deletedetails(ClgMasterCourseBranchMapDTO id);
        ClgMasterCourseBranchMapDTO showmodaldetails(ClgMasterCourseBranchMapDTO id);
        ClgMasterCourseBranchMapDTO deactivesem(ClgMasterCourseBranchMapDTO id);
        ClgMasterCourseBranchMapDTO edit(ClgMasterCourseBranchMapDTO id);
        
    }
    
}
