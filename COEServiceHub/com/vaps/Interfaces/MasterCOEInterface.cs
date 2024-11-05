using PreadmissionDTOs.com.vaps.COE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoeServiceHub.com.vaps.Interfaces
{
    public interface MasterCOEInterface
    {
        MasterCOEDTO savedetail1(MasterCOEDTO objcategory);
        MasterCOEDTO savedetail2(MasterCOEDTO objcategory);
        MasterCOEDTO geteventdetails(MasterCOEDTO objcategory);
        MasterCOEDTO deactivate1(MasterCOEDTO data);
        MasterCOEDTO deactivate2(MasterCOEDTO data);
        MasterCOEDTO getdetails(int id);
        MasterCOEDTO getpageedit1(int id);
        MasterCOEDTO getpageedit2(int id);
        MasterCOEDTO getalldetailsviewrecords1(int id);
        MasterCOEDTO getalldetailsviewrecords2(int id);
        MasterCOEDTO deleterec(int id);


    }
}
