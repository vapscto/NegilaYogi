using PreadmissionDTOs.com.vaps.Portals.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.IVRM.Interfaces
{
   public interface IVRM_PrincipalMappingInterface
    {
        Task<IVRM_PrincipalMappingDTO> GetdetailsAsync(IVRM_PrincipalMappingDTO data);

        IVRM_PrincipalMappingDTO saveclsdata(IVRM_PrincipalMappingDTO data);

        IVRM_PrincipalMappingDTO saveprncplstaf(IVRM_PrincipalMappingDTO data);

        IVRM_PrincipalMappingDTO deactivehod(IVRM_PrincipalMappingDTO data);

        IVRM_PrincipalMappingDTO Deactivatestaf(IVRM_PrincipalMappingDTO data);

        IVRM_PrincipalMappingDTO editprincipledata(IVRM_PrincipalMappingDTO data);

        IVRM_PrincipalMappingDTO editprinciplestaffdata(IVRM_PrincipalMappingDTO data);

        IVRM_PrincipalMappingDTO onmodelclick(IVRM_PrincipalMappingDTO data);

        IVRM_PrincipalMappingDTO Deactivateclass(IVRM_PrincipalMappingDTO data);


    }
}
