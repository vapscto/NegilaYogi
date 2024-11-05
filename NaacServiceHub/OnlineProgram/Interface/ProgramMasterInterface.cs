using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.OnlineProgram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.OnlineProgram.Interfaces
{
    public interface ProgramMasterInterface
    {

        OnlineProgramDTO getloaddata(OnlineProgramDTO data);
        OnlineProgramDTO savedatalevel(OnlineProgramDTO data);
        OnlineProgramDTO savedatatype(OnlineProgramDTO data);
        OnlineProgramDTO editlevel(OnlineProgramDTO data);
        OnlineProgramDTO deactivelevel(OnlineProgramDTO data);
        OnlineProgramDTO edittype(OnlineProgramDTO data);
        OnlineProgramDTO deactivetype(OnlineProgramDTO data);


    }
}
