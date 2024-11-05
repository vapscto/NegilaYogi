using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{

    public  interface StaffnStudentReportInterface
    {
       
        TT_StaffnStudentReportDTO getdetails(int id);
        TT_StaffnStudentReportDTO getreport(TT_StaffnStudentReportDTO org);


    }
}
