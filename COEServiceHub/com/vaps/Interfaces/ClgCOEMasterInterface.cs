using PreadmissionDTOs.com.vaps.College.COE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COEServiceHub.com.vaps.Interfaces
{
    public interface ClgCOEMasterInterface
    {
        ClgMasterCOEDTO courseselect(ClgMasterCOEDTO objcategory);
        ClgMasterCOEDTO branchselect(ClgMasterCOEDTO objcategory);

        ClgMasterCOEDTO savedetail1(ClgMasterCOEDTO objcategory);
        ClgMasterCOEDTO savedetail2(ClgMasterCOEDTO objcategory);
        ClgMasterCOEDTO geteventdetails(ClgMasterCOEDTO objcategory);
        ClgMasterCOEDTO deactivate1(ClgMasterCOEDTO data);
        ClgMasterCOEDTO deactivate2(ClgMasterCOEDTO data);
        ClgMasterCOEDTO getdetails(ClgMasterCOEDTO data);
        ClgMasterCOEDTO getpageedit1(int id);
        ClgMasterCOEDTO getpageedit2(int id);
        ClgMasterCOEDTO getalldetailsviewrecords1(int id);
        ClgMasterCOEDTO getalldetailsviewrecords2(int id);
        ClgMasterCOEDTO deleterec(int id);
    }
}
