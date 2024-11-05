using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{

    public  interface FixingInterface
    {

        TTFixingDTO savedetail1(TTFixingDTO objday);
        TTFixingDTO savedetail2(TTFixingDTO objday);
        TTFixingDTO savedetail3(TTFixingDTO objday);
        TTFixingDTO savedetail4(TTFixingDTO objday);
        TTFixingDTO savedetail5(TTFixingDTO objday);
        TTFixingDTO getpageedit1(int id);
        TTFixingDTO getpageedit2(int id);
        TTFixingDTO getpageedit3(int id);
        TTFixingDTO getpageedit4(int id);
        TTFixingDTO getpageedit5(int id);
        TTFixingDTO getalldetailsviewrecords2(int id);
        TTFixingDTO getalldetailsviewrecords3(int id);
        TTFixingDTO getalldetailsviewrecords4(int id);
        TTFixingDTO getalldetailsviewrecords5(int id);
        TTFixingDTO deactivate1(TTFixingDTO data);
        TTFixingDTO deactivate2(TTFixingDTO data);
        TTFixingDTO deactivate3(TTFixingDTO data);
        TTFixingDTO deactivate4(TTFixingDTO data);
        TTFixingDTO deactivate5(TTFixingDTO data);
        TTFixingDTO savedetail(TTFixingDTO objperiod); 
         TTFixingDTO getdetails(TTFixingDTO data);
        TTFixingDTO getcategories(TTFixingDTO data);
        TTFixingDTO get_cls_sec_subs(TTFixingDTO data);
        TTFixingDTO get_cls_sec_staffs(TTFixingDTO data);
        TTFixingDTO getalldetailsviewrecords(int id);
        TTFixingDTO getclasses(TTFixingDTO data);
        TTFixingDTO getperiods(TTFixingDTO data);
        TTFixingDTO getstaff(TTFixingDTO data);
        TTFixingDTO getsubjects(TTFixingDTO data);
        TTFixingDTO deactivate(TTFixingDTO data);
        TTFixingDTO getpageedit(int id);
        TTFixingDTO deleterec(int id);

    }
}
