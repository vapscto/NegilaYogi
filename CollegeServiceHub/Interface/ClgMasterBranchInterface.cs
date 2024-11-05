using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface ClgMasterBranchInterface
    {
        ClgMasterBranchDTO getalldetails(ClgMasterBranchDTO data);
        ClgMasterBranchDTO savebranch(ClgMasterBranchDTO data);
        ClgMasterBranchDTO editbranch(ClgMasterBranchDTO data);
        ClgMasterBranchDTO activedeactivebranch(ClgMasterBranchDTO data);
        ClgMasterBranchDTO saveorder(ClgMasterBranchDTO data);
        
    }
    
}
