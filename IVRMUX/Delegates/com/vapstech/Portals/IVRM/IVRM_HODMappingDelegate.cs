using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.HOD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Portals.IVRM
{
    public class IVRM_HODMappingDelegate
    {

        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<IVRM_HodMappingDTO, IVRM_HodMappingDTO> COMMM = new CommonDelegate<IVRM_HodMappingDTO, IVRM_HodMappingDTO>();

        public IVRM_HodMappingDTO Getdetails(IVRM_HodMappingDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_HODMappingFacade/Getdetails/");
        }
        

        public IVRM_HodMappingDTO saveclsdata(IVRM_HodMappingDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_HODMappingFacade/saveclsdata/");
        }
        public IVRM_HodMappingDTO savehodstaf(IVRM_HodMappingDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_HODMappingFacade/savehodstaf/");
        }
        public IVRM_HodMappingDTO deactivehod(IVRM_HodMappingDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_HODMappingFacade/deactivehod/");
        }

        public IVRM_HodMappingDTO Deactivatestaf(IVRM_HodMappingDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_HODMappingFacade/Deactivatestaf/");
        }
        public IVRM_HodMappingDTO editHoddata(IVRM_HodMappingDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_HODMappingFacade/editHoddata/");
        }

        public IVRM_HodMappingDTO editHodStaffdata(IVRM_HodMappingDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_HODMappingFacade/editHodStaffdata/");
        }

        public IVRM_HodMappingDTO onmodelclick(IVRM_HodMappingDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_HODMappingFacade/onmodelclick/");
        }
        public IVRM_HodMappingDTO Deactivateclass(IVRM_HodMappingDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_HODMappingFacade/Deactivateclass/");
        }
        
    }
}
