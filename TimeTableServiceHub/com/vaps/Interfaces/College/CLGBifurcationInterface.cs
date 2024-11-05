using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.Interfaces
{
    public interface CLGBifurcationInterface
    {
      
        CLGBifurcationDTO editbiff(CLGBifurcationDTO data);
        CLGBifurcationDTO deactivatebiff(CLGBifurcationDTO data);
        CLGBifurcationDTO viewrecordspopup(CLGBifurcationDTO data);
        CLGBifurcationDTO getalldetails(CLGBifurcationDTO data);
        CLGBifurcationDTO editDay(CLGBifurcationDTO data);
        CLGBifurcationDTO getBranch(CLGBifurcationDTO data);
        CLGBifurcationDTO savedetailBiff(CLGBifurcationDTO data);
       
    }
}
