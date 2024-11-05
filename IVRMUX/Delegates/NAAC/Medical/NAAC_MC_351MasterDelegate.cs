using CommonLibrary;
using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Medical
{
    public class NAAC_MC_351MasterDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<NAAC_MC_351_CollaborationActivities_DTO, NAAC_MC_351_CollaborationActivities_DTO> COMMM = new CommonDelegate<NAAC_MC_351_CollaborationActivities_DTO, NAAC_MC_351_CollaborationActivities_DTO>();

        public NAAC_MC_351_CollaborationActivities_DTO loaddata(NAAC_MC_351_CollaborationActivities_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_MC_351MasterFacade/loaddata/");
        }
        public NAAC_MC_351_CollaborationActivities_DTO savedata(NAAC_MC_351_CollaborationActivities_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_MC_351MasterFacade/savedata");
        }
        public NAAC_MC_351_CollaborationActivities_DTO editdata(NAAC_MC_351_CollaborationActivities_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_MC_351MasterFacade/editdata");
        }
        public NAAC_MC_351_CollaborationActivities_DTO deactivY(NAAC_MC_351_CollaborationActivities_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_MC_351MasterFacade/deactivY");
        }
        public NAAC_MC_351_CollaborationActivities_DTO viewuploadflies(NAAC_MC_351_CollaborationActivities_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_MC_351MasterFacade/viewuploadflies");
        }
        public NAAC_MC_351_CollaborationActivities_DTO deleteuploadfile(NAAC_MC_351_CollaborationActivities_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_MC_351MasterFacade/deleteuploadfile");
        }
    }
}
