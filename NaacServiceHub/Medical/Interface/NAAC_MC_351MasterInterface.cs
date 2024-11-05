using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Medical.Interface
{
   public interface NAAC_MC_351MasterInterface
    {
        NAAC_MC_351_CollaborationActivities_DTO loaddata(NAAC_MC_351_CollaborationActivities_DTO data);
        NAAC_MC_351_CollaborationActivities_DTO savedata(NAAC_MC_351_CollaborationActivities_DTO data);
        NAAC_MC_351_CollaborationActivities_DTO editdata(NAAC_MC_351_CollaborationActivities_DTO data);
        NAAC_MC_351_CollaborationActivities_DTO deactivY(NAAC_MC_351_CollaborationActivities_DTO data);
        NAAC_MC_351_CollaborationActivities_DTO viewuploadflies(NAAC_MC_351_CollaborationActivities_DTO data);
        NAAC_MC_351_CollaborationActivities_DTO deleteuploadfile(NAAC_MC_351_CollaborationActivities_DTO data);
    }
}
