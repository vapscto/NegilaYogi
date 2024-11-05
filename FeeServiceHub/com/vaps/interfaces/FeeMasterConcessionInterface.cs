using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.interfaces
{
     public interface FeeMasterConcessionInterface
    {

      

        FeeMasterConcessionDTO getdata(FeeMasterConcessionDTO id);

        FeeMasterConcessionDTO savedata(FeeMasterConcessionDTO data);

        FeeMasterConcessionDTO activedeactive(FeeMasterConcessionDTO data);

        FeeMasterConcessionDTO editdata(FeeMasterConcessionDTO data);
        FeeMasterConcessionDTO savedata2(FeeMasterConcessionDTO data);
        FeeMasterConcessionDTO deactive2(FeeMasterConcessionDTO data);
        FeeMasterConcessionDTO edit2(FeeMasterConcessionDTO data);
        FeeMasterConcessionDTO gethead(FeeMasterConcessionDTO data);
        FeeMasterConcessionDTO savedata3(FeeMasterConcessionDTO data);
        FeeMasterConcessionDTO edit3(FeeMasterConcessionDTO data);
        FeeMasterConcessionDTO deactive3(FeeMasterConcessionDTO data);

    }
}
