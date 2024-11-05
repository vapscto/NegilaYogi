using CommonLibrary;
using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Medical
{
    public class NAAC_MC_VACcommonDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<NAAC_MC_VACcommon_DTO, NAAC_MC_VACcommon_DTO> COMMM = new CommonDelegate<NAAC_MC_VACcommon_DTO, NAAC_MC_VACcommon_DTO>();

       
        public NAAC_MC_VACcommon_DTO loaddata(NAAC_MC_VACcommon_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_MC_VACcommonFacade/loaddata/");
        }
        public NAAC_MC_VACcommon_DTO savedata141(NAAC_MC_VACcommon_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_MC_VACcommonFacade/savedata141/");
        }
        public NAAC_MC_VACcommon_DTO editdata141(NAAC_MC_VACcommon_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_MC_VACcommonFacade/editdata141/");
        }
        public NAAC_MC_VACcommon_DTO savedata142(NAAC_MC_VACcommon_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_MC_VACcommonFacade/savedata142/");
        }

        public NAAC_MC_VACcommon_DTO M_savedata221(NAAC_MC_VACcommon_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_MC_VACcommonFacade/M_savedata221/");
        }
        public NAAC_MC_VACcommon_DTO M_savedata232(NAAC_MC_VACcommon_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_MC_VACcommonFacade/M_savedata232/");
        }
        public NAAC_MC_VACcommon_DTO M_savedata254(NAAC_MC_VACcommon_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_MC_VACcommonFacade/M_savedata254/");
        }
        
    }
}
