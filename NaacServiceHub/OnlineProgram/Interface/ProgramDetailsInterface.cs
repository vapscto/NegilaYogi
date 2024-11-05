using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.OnlineProgram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.OnlineProgram.Interfaces
{
   public interface ProgramDetailsInterface
    {
        OnlineProgramDTO getloaddata(OnlineProgramDTO data);
        OnlineProgramDTO savedetail(OnlineProgramDTO data);
        
        OnlineProgramDTO getalldetailsviewrecords(OnlineProgramDTO data);

        OnlineProgramDTO deleterecord(OnlineProgramDTO data);
        
    }
}
