using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{

    public  interface ClassMasterInterface
    {
        TTClassMasterDTO savedetail(TTClassMasterDTO objperiod); 
         TTClassMasterDTO getdetails(TTClassMasterDTO data);
        TTClassMasterDTO getcategories(TTClassMasterDTO data);
        TTClassMasterDTO getalldetailsviewrecords(int id);
        TTClassMasterDTO getclasses(TTClassMasterDTO data);
        TTClassMasterDTO deactivate(TTClassMasterDTO data);
        TTClassMasterDTO getpageedit(int id);
        TTClassMasterDTO deleterec(int id);

    }
}
