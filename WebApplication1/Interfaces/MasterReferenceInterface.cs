using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
   public interface MasterReferenceInterface
    {
         MasterRefernceDTO SaveMasterRefernceDetails(MasterRefernceDTO master);
        MasterRefernceDTO GetMasterReferncDetails(MasterRefernceDTO master);
        MasterRefernceDTO DeleteMasterReferncDetails(int id);
        MasterRefernceDTO EditMasterRefDetails(int id);
        MasterRefernceDTO getsearchdata(int id, MasterRefernceDTO org);
    }
}
