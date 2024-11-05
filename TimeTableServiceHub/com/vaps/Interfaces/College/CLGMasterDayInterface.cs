using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.Interfaces
{
    public interface CLGMasterDayInterface
    {
      
        CLGMasterDayDTO savesemday(CLGMasterDayDTO data);
        CLGMasterDayDTO daydeactive(CLGMasterDayDTO data);
        CLGMasterDayDTO deactivecrsday(CLGMasterDayDTO data);
        CLGMasterDayDTO getorder(CLGMasterDayDTO data);
        CLGMasterDayDTO saveorder(CLGMasterDayDTO data);
        CLGMasterDayDTO getalldetails(CLGMasterDayDTO data);
        CLGMasterDayDTO editDay(CLGMasterDayDTO data);
        CLGMasterDayDTO getBranch(CLGMasterDayDTO data);
        CLGMasterDayDTO saveday(CLGMasterDayDTO data);
       
    }
}
