using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
     public  interface CLGRegNoFormatInterface
    {
        CLGAdm_College_RegNo_FormatDTO Savedetails(CLGAdm_College_RegNo_FormatDTO id);
        CLGAdm_College_RegNo_FormatDTO getalldetails(int id);
        CLGAdm_College_RegNo_FormatDTO Deletedetails(CLGAdm_College_RegNo_FormatDTO id);
    }
}
