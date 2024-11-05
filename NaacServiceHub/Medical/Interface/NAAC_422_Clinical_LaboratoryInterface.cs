using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Medical.Interface
{
   public interface NAAC_422_Clinical_LaboratoryInterface
    {
        NAAC_MC_422_Clinical_Laboratory_DTO loaddata(NAAC_MC_422_Clinical_Laboratory_DTO data);
        NAAC_MC_422_Clinical_Laboratory_DTO savedata(NAAC_MC_422_Clinical_Laboratory_DTO data);
        NAAC_MC_422_Clinical_Laboratory_DTO editdata(NAAC_MC_422_Clinical_Laboratory_DTO data);
        NAAC_MC_422_Clinical_Laboratory_DTO deactive_Y(NAAC_MC_422_Clinical_Laboratory_DTO data);
        NAAC_MC_422_Clinical_Laboratory_DTO viewuploadflies(NAAC_MC_422_Clinical_Laboratory_DTO data);
        NAAC_MC_422_Clinical_Laboratory_DTO deleteuploadfile(NAAC_MC_422_Clinical_Laboratory_DTO data);

    }
}
