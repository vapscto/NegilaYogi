using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace TimeTableServiceHub.com.vaps.Interfaces
{
  public  interface LabconstraintsInterface
    {
        TT_LABLIB_DTO savedetail(TT_LABLIB_DTO objcategory);
        TT_LABLIB_DTO getdetails(int id);
        TT_LABLIB_DTO getpageedit(int id);
        TT_LABLIB_DTO deleterec(int id);
        TT_LABLIB_DTO getclass_catg(TT_LABLIB_DTO objcategory);
        TT_LABLIB_DTO get_catg(TT_LABLIB_DTO objcategory);
        TT_LABLIB_DTO getalldetailsviewrecords(int id);
        TT_LABLIB_DTO deactivate(TT_LABLIB_DTO id);
    }
}
