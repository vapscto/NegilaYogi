using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.OnlineProgram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace NaacServiceHub.OnlineProgram.Interfaces
{
    public interface CompletedEventInterface
    {
        OnlineProgramDTO getloaddata(OnlineProgramDTO data);
        OnlineProgramDTO Savedata(OnlineProgramDTO data);
        OnlineProgramDTO getdetails(OnlineProgramDTO data);
        OnlineProgramDTO deactivate(OnlineProgramDTO data);
    }
}
