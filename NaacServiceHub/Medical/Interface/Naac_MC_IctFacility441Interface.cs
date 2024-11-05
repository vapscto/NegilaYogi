using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Medical.Interface
{
  public  interface Naac_MC_IctFacility441Interface
    {

        Naac_MC_IctFacility441_DTO loaddata(Naac_MC_IctFacility441_DTO data);
        Naac_MC_IctFacility441_DTO save(Naac_MC_IctFacility441_DTO data);

        Naac_MC_IctFacility441_DTO EditData(Naac_MC_IctFacility441_DTO data);
        Naac_MC_IctFacility441_DTO viewuploadflies(Naac_MC_IctFacility441_DTO data);
        Naac_MC_IctFacility441_DTO deleteuploadfile(Naac_MC_IctFacility441_DTO obj);


    }
}
