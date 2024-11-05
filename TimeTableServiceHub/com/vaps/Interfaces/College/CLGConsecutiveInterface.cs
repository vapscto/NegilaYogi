using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.Interfaces
{
    public interface CLGConsecutiveInterface
    {
      
       
        CLGConsecutiveDTO deactivate(CLGConsecutiveDTO data);
    
        CLGConsecutiveDTO getalldetails(CLGConsecutiveDTO data);
        CLGConsecutiveDTO editconv(CLGConsecutiveDTO data);
       
        CLGConsecutiveDTO savedetail(CLGConsecutiveDTO data);
       
    }
}
