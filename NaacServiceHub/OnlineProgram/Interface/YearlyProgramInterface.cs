using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.OnlineProgram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.OnlineProgram.Interfaces
{
    public interface YearlyProgramInterface
    {
        OnlineProgramDTO getloaddata(OnlineProgramDTO data);
        OnlineProgramDTO Savedata(OnlineProgramDTO data);
        OnlineProgramDTO getdetails(OnlineProgramDTO data);
        OnlineProgramDTO delete(OnlineProgramDTO data);
        OnlineProgramDTO editguest(OnlineProgramDTO data);
        OnlineProgramDTO viewuploadflies(OnlineProgramDTO data);
        OnlineProgramDTO removeNewsiblinguest(OnlineProgramDTO data);

    }
}
