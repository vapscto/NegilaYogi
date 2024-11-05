using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Interfaces
{
    public interface ProgramMasterInterface
    {
        ProgramMasterDTO getDetails(ProgramMasterDTO data);
        ProgramMasterDTO saveRecord(ProgramMasterDTO data);
        ProgramMasterDTO EditDetails(int data);
        ProgramMasterDTO deactivate(ProgramMasterDTO data);
    }
}
