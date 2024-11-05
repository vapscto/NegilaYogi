using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Portals.IVRM
{
    public class IVRM_PrincipalMappingDelegate
    {

        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<IVRM_PrincipalMappingDTO, IVRM_PrincipalMappingDTO> COMMM = new CommonDelegate<IVRM_PrincipalMappingDTO, IVRM_PrincipalMappingDTO>();

        public IVRM_PrincipalMappingDTO Getdetails(IVRM_PrincipalMappingDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_PrincipalMappingFacade/Getdetails/");
        }

        public IVRM_PrincipalMappingDTO saveclsdata(IVRM_PrincipalMappingDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_PrincipalMappingFacade/saveclsdata/");
        }

        public IVRM_PrincipalMappingDTO saveprncplstaf(IVRM_PrincipalMappingDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_PrincipalMappingFacade/saveprncplstaf/");
        }

        public IVRM_PrincipalMappingDTO deactivehod(IVRM_PrincipalMappingDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_PrincipalMappingFacade/deactivehod/");
        }

        public IVRM_PrincipalMappingDTO Deactivatestaf(IVRM_PrincipalMappingDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_PrincipalMappingFacade/Deactivatestaf/");
        }

        public IVRM_PrincipalMappingDTO editprincipledata(IVRM_PrincipalMappingDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_PrincipalMappingFacade/editprincipledata/");
        }

        public IVRM_PrincipalMappingDTO editprinciplestaffdata(IVRM_PrincipalMappingDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_PrincipalMappingFacade/editprinciplestaffdata/");
        }

        public IVRM_PrincipalMappingDTO onmodelclick(IVRM_PrincipalMappingDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_PrincipalMappingFacade/onmodelclick/");
        }
        public IVRM_PrincipalMappingDTO Deactivateclass(IVRM_PrincipalMappingDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_PrincipalMappingFacade/Deactivateclass/");
        }


    }
}
