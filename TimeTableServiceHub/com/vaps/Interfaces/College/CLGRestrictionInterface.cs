using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.Interfaces
{
    public interface CLGRestrictionInterface
    {
        //TAB1 START FIXING DAY PERIOD
        CLGRestrictionDTO deactivatetab1(CLGRestrictionDTO data);
        CLGRestrictionDTO getalldetails(CLGRestrictionDTO data);
        CLGRestrictionDTO edittab1(CLGRestrictionDTO data);
        CLGRestrictionDTO savetab1(CLGRestrictionDTO data);
        //TAB1 END FIXING DAY PERIOD

        //TAB2 START FIXING DAY STAFF
        CLGRestrictionDTO savetab2(CLGRestrictionDTO data);
        CLGRestrictionDTO viewtab2grid(CLGRestrictionDTO data);
        CLGRestrictionDTO gettab2editdata(CLGRestrictionDTO data);
        CLGRestrictionDTO deactivatetab2(CLGRestrictionDTO data);
        //TAB2 END FIXING DAY STAFF

        //TAB3 START FIXING DAY SUBJECT
        CLGRestrictionDTO savetab3(CLGRestrictionDTO data);
        CLGRestrictionDTO viewtab3grid(CLGRestrictionDTO data);
        CLGRestrictionDTO edittab3(CLGRestrictionDTO data);
        CLGRestrictionDTO deactivatetab3(CLGRestrictionDTO data);

        //TAB2 END FIXING DAY SUBJECT

        //TAB4 START FIXING PERIOD STAFF
        CLGRestrictionDTO savetab4(CLGRestrictionDTO data);
        CLGRestrictionDTO viewtab4(CLGRestrictionDTO data);
        CLGRestrictionDTO edittab4(CLGRestrictionDTO data);
        CLGRestrictionDTO deactivatetab4(CLGRestrictionDTO data);


        //TAB4 END FIXING PERIOD STAFF
        //TAB5 START FIXING PERIOD SUBJECT
        CLGRestrictionDTO savetab5(CLGRestrictionDTO data);
        CLGRestrictionDTO viewtab5(CLGRestrictionDTO data);
        CLGRestrictionDTO edittab5(CLGRestrictionDTO data);
        CLGRestrictionDTO deactivatetab5(CLGRestrictionDTO data);

        //TAB5 END FIXING PERIOD SUBJECT

    }
}
