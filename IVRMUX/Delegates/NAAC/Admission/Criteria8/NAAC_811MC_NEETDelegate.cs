using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission.Criteria8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission.Criteria8
{
   
    public class NAAC_811MC_NEETDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<NAAC_811MC_NEET_DTO, NAAC_811MC_NEET_DTO> COMMM = new CommonDelegate<NAAC_811MC_NEET_DTO, NAAC_811MC_NEET_DTO>();

        public NAAC_811MC_NEET_DTO loaddata(NAAC_811MC_NEET_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_811MC_NEETFacade/loaddata/");
        }
        public NAAC_811MC_NEET_DTO savedata(NAAC_811MC_NEET_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_811MC_NEETFacade/savedata/");
        }
        public NAAC_811MC_NEET_DTO editdata(NAAC_811MC_NEET_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_811MC_NEETFacade/editdata/");
        }
        public NAAC_811MC_NEET_DTO deactivY(NAAC_811MC_NEET_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_811MC_NEETFacade/deactivY/");
        }
        public NAAC_811MC_NEET_DTO viewuploadflies(NAAC_811MC_NEET_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_811MC_NEETFacade/viewuploadflies");
        }
        public NAAC_811MC_NEET_DTO deleteuploadfile(NAAC_811MC_NEET_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_811MC_NEETFacade/deleteuploadfile");
        }
        public NAAC_811MC_NEET_DTO getcomment(NAAC_811MC_NEET_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_811MC_NEETFacade/getcomment");
        }
        public NAAC_811MC_NEET_DTO getfilecomment(NAAC_811MC_NEET_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_811MC_NEETFacade/getfilecomment");
        }
        public NAAC_811MC_NEET_DTO savecomments(NAAC_811MC_NEET_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_811MC_NEETFacade/savecomments");
        }
        public NAAC_811MC_NEET_DTO savefilewisecomments(NAAC_811MC_NEET_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_811MC_NEETFacade/savefilewisecomments");
        }
    }
}
