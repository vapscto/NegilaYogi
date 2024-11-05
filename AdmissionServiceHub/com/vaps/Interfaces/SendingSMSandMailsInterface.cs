using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
namespace AdmissionServiceHub.com.vaps.Interfaces
{
   public  interface SendingSMSandMailsInterface
    {
        CommonDTO getdetails(int id);

        CommonDTO getdetailsstudentorstaff(CommonDTO data);
    }
}
