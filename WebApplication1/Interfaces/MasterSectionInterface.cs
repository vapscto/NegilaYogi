using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
   public interface MasterSectionInterface
    {
   MasterSectionDTO SaveMasterscetionDetails(MasterSectionDTO master);
        MasterSectionDTO DeleteMasterscetionDetails(MasterSectionDTO id);
        MasterSectionDTO EditMasterscetionDetails(int id);
        MasterSectionDTO GetMasterscetionDetails(int mi_id);

        MasterSectionDTO getsearchdata(int id, MasterSectionDTO org);
    }
}
