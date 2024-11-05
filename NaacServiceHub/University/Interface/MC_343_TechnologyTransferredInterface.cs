using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.University.Interface
{
  public  interface MC_343_TechnologyTransferredInterface
    {
        MC_343_TechnologyTransferredDTO loaddata(MC_343_TechnologyTransferredDTO data);
        MC_343_TechnologyTransferredDTO save(MC_343_TechnologyTransferredDTO data);
        MC_343_TechnologyTransferredDTO deactive(MC_343_TechnologyTransferredDTO data);
        MC_343_TechnologyTransferredDTO EditData(MC_343_TechnologyTransferredDTO data);
        MC_343_TechnologyTransferredDTO viewuploadflies(MC_343_TechnologyTransferredDTO data);
        MC_343_TechnologyTransferredDTO deleteuploadfile(MC_343_TechnologyTransferredDTO data);
    }
}
