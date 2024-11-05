using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{

    public  interface RestrictionInterface
    {

        TTRestrictionDTO savedetail1(TTRestrictionDTO objday);
        TTRestrictionDTO savedetail2(TTRestrictionDTO objday);
        TTRestrictionDTO savedetail3(TTRestrictionDTO objday);
        TTRestrictionDTO savedetail4(TTRestrictionDTO objday);
        TTRestrictionDTO savedetail5(TTRestrictionDTO objday);
        TTRestrictionDTO getpageedit1(int id);
        TTRestrictionDTO getpageedit2(int id);
        TTRestrictionDTO getpageedit3(int id);
        TTRestrictionDTO getpageedit4(int id);
        TTRestrictionDTO getpageedit5(int id);
        TTRestrictionDTO getalldetailsviewrecords2(int id);
        TTRestrictionDTO getalldetailsviewrecords3(int id);
        TTRestrictionDTO getalldetailsviewrecords4(int id);
        TTRestrictionDTO getalldetailsviewrecords5(int id);
        TTRestrictionDTO deactivate1(TTRestrictionDTO data);
        TTRestrictionDTO deactivate2(TTRestrictionDTO data);
        TTRestrictionDTO deactivate3(TTRestrictionDTO data);
        TTRestrictionDTO deactivate4(TTRestrictionDTO data);
        TTRestrictionDTO deactivate5(TTRestrictionDTO data);
        TTRestrictionDTO savedetail(TTRestrictionDTO objperiod); 
         TTRestrictionDTO getdetails(TTRestrictionDTO data);
        TTRestrictionDTO getcategories(TTRestrictionDTO data);
        TTRestrictionDTO get_cls_sec_subs(TTRestrictionDTO data);
        TTRestrictionDTO get_cls_sec_staffs(TTRestrictionDTO data);
        TTRestrictionDTO getalldetailsviewrecords(int id);
        TTRestrictionDTO getclasses(TTRestrictionDTO data);
        TTRestrictionDTO getperiods(TTRestrictionDTO data);
        TTRestrictionDTO getstaff(TTRestrictionDTO data);
        TTRestrictionDTO getsubjects(TTRestrictionDTO data);
        TTRestrictionDTO deactivate(TTRestrictionDTO data);
        TTRestrictionDTO getpageedit(int id);
        TTRestrictionDTO deleterec(int id);

    }
}
