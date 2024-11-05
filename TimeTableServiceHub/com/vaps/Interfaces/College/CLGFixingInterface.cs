using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.Interfaces
{
    public interface CLGFixingInterface
    {
        //TAB1 START FIXING DAY PERIOD
        CLGFixingDTO deactivatetab1(CLGFixingDTO data);
        CLGFixingDTO getalldetails(CLGFixingDTO data);
        CLGFixingDTO edittab1(CLGFixingDTO data);
        CLGFixingDTO savetab1(CLGFixingDTO data);
        //TAB1 END FIXING DAY PERIOD

        //TAB2 START FIXING DAY STAFF
        CLGFixingDTO savetab2(CLGFixingDTO data);
        CLGFixingDTO viewtab2grid(CLGFixingDTO data);
        CLGFixingDTO gettab2editdata(CLGFixingDTO data);
        CLGFixingDTO deactivatetab2(CLGFixingDTO data);
        //TAB2 END FIXING DAY STAFF

        //TAB3 START FIXING DAY SUBJECT
        CLGFixingDTO savetab3(CLGFixingDTO data);
        CLGFixingDTO viewtab3grid(CLGFixingDTO data);
        CLGFixingDTO edittab3(CLGFixingDTO data);
        CLGFixingDTO deactivatetab3(CLGFixingDTO data);

        //TAB2 END FIXING DAY SUBJECT

        //TAB4 START FIXING PERIOD STAFF
        CLGFixingDTO savetab4(CLGFixingDTO data);
        CLGFixingDTO viewtab4(CLGFixingDTO data);
        CLGFixingDTO edittab4(CLGFixingDTO data);
        CLGFixingDTO deactivatetab4(CLGFixingDTO data);


        //TAB4 END FIXING PERIOD STAFF
        //TAB5 START FIXING PERIOD SUBJECT
        CLGFixingDTO savetab5(CLGFixingDTO data);
        CLGFixingDTO viewtab5(CLGFixingDTO data);
        CLGFixingDTO edittab5(CLGFixingDTO data);
        CLGFixingDTO deactivatetab5(CLGFixingDTO data);

        //TAB5 END FIXING PERIOD SUBJECT

    }
}
