using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace CollegeServiceHub.Interface
{
    public interface ClgMasterCategoryInterface
    {
        ClgMasterCategoryDTO Savedetails(ClgMasterCategoryDTO cat);
        ClgMasterCategoryDTO getalldetails(int id);       
        ClgMasterCategoryDTO Deletedetails(ClgMasterCategoryDTO data);
        ClgMasterCategoryDTO deactivate(ClgMasterCategoryDTO data);
        
    }
}
