using PreadmissionDTOs.com.vaps.PDA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDAServiceHub.com.vaps.interfaces
{
   public interface PDAMasterHeadInterface
    {
        PdaDTO getdetails(PdaDTO data);

        PdaDTO savedetails(PdaDTO data);

        PdaDTO getpageedit(PdaDTO data);
        PdaDTO deactivate(PdaDTO data);





    }
}
