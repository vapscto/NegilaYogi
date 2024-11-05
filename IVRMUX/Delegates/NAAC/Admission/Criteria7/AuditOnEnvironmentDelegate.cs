using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class AuditOnEnvironmentDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<NAAC_MC_716_AuditOnEnvironment_DTO, NAAC_MC_716_AuditOnEnvironment_DTO> COMMM = new CommonDelegate<NAAC_MC_716_AuditOnEnvironment_DTO, NAAC_MC_716_AuditOnEnvironment_DTO>();

        public NAAC_MC_716_AuditOnEnvironment_DTO loaddata(NAAC_MC_716_AuditOnEnvironment_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "AuditOnEnvironmentFacade/loaddata/");
        }

        public NAAC_MC_716_AuditOnEnvironment_DTO savedatatab1(NAAC_MC_716_AuditOnEnvironment_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "AuditOnEnvironmentFacade/savedatatab1");
        }

        public NAAC_MC_716_AuditOnEnvironment_DTO editTab1(NAAC_MC_716_AuditOnEnvironment_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "AuditOnEnvironmentFacade/editTab1");
        }

        public NAAC_MC_716_AuditOnEnvironment_DTO deactivYTab1(NAAC_MC_716_AuditOnEnvironment_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "AuditOnEnvironmentFacade/deactivYTab1");
        }

        public NAAC_MC_716_AuditOnEnvironment_DTO deleteuploadfile(NAAC_MC_716_AuditOnEnvironment_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "AuditOnEnvironmentFacade/deleteuploadfile");
        }

        public NAAC_MC_716_AuditOnEnvironment_DTO getData(NAAC_MC_716_AuditOnEnvironment_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "AuditOnEnvironmentFacade/getData");
        }

    }
}
