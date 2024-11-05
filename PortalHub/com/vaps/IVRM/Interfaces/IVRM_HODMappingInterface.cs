using PreadmissionDTOs.com.vaps.Portals.HOD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.IVRM.Interfaces
{
   public interface IVRM_HODMappingInterface
    {
        Task<IVRM_HodMappingDTO> GetdetailsAsync(IVRM_HodMappingDTO data);

        IVRM_HodMappingDTO saveclsdata(IVRM_HodMappingDTO data);

        IVRM_HodMappingDTO savehodstaf(IVRM_HodMappingDTO data);

        IVRM_HodMappingDTO deactivehod(IVRM_HodMappingDTO data);

        IVRM_HodMappingDTO Deactivatestaf(IVRM_HodMappingDTO data);

        IVRM_HodMappingDTO editHoddata(IVRM_HodMappingDTO data);

        IVRM_HodMappingDTO editHodStaffdata(IVRM_HodMappingDTO data);

        IVRM_HodMappingDTO onmodelclick(IVRM_HodMappingDTO data);

        IVRM_HodMappingDTO Deactivateclass(IVRM_HodMappingDTO data);


        

    }
}
