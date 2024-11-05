using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class LEDBulbsDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<NAAC_AC_714_LEDBulbs_DTO, NAAC_AC_714_LEDBulbs_DTO> COMMM = new CommonDelegate<NAAC_AC_714_LEDBulbs_DTO, NAAC_AC_714_LEDBulbs_DTO>();

        public NAAC_AC_714_LEDBulbs_DTO loaddata(NAAC_AC_714_LEDBulbs_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "LEDBulbsFacade/loaddata/");
        }

        public NAAC_AC_714_LEDBulbs_DTO savedatatab1(NAAC_AC_714_LEDBulbs_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "LEDBulbsFacade/savedatatab1");
        }
        public NAAC_AC_714_LEDBulbs_DTO editTab1(NAAC_AC_714_LEDBulbs_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "LEDBulbsFacade/editTab1");
        }
        public NAAC_AC_714_LEDBulbs_DTO savemedicaldatawisecommentsLEDbulb(NAAC_AC_714_LEDBulbs_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "LEDBulbsFacade/savemedicaldatawisecommentsLEDbulb");
        }
        public NAAC_AC_714_LEDBulbs_DTO savefilewisecommentsLEDbulb(NAAC_AC_714_LEDBulbs_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "LEDBulbsFacade/savefilewisecommentsLEDbulb");
        }
        public NAAC_AC_714_LEDBulbs_DTO getcommentLEDbulb(NAAC_AC_714_LEDBulbs_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "LEDBulbsFacade/getcommentLEDbulb");
        }
        public NAAC_AC_714_LEDBulbs_DTO getfilecommentLEDbulb(NAAC_AC_714_LEDBulbs_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "LEDBulbsFacade/getfilecommentLEDbulb");
        }
        public NAAC_AC_714_LEDBulbs_DTO savemedicaldatawisecomments(NAAC_AC_714_LEDBulbs_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "LEDBulbsFacade/savemedicaldatawisecomments");
        }
        public NAAC_AC_714_LEDBulbs_DTO getcomment(NAAC_AC_714_LEDBulbs_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "LEDBulbsFacade/getcomment");
        }

        public NAAC_AC_714_LEDBulbs_DTO deactivYTab1(NAAC_AC_714_LEDBulbs_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "LEDBulbsFacade/deactivYTab1");
        }

        public NAAC_AC_714_LEDBulbs_DTO deleteuploadfile(NAAC_AC_714_LEDBulbs_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "LEDBulbsFacade/deleteuploadfile");
        }

        public NAAC_AC_714_LEDBulbs_DTO getData(NAAC_AC_714_LEDBulbs_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "LEDBulbsFacade/getData");
        }

        //MC
        public NAAC_AC_714_LEDBulbs_DTO getDataMCwater(NAAC_AC_714_LEDBulbs_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "LEDBulbsFacade/getDataMCwater");
        }
        public NAAC_AC_714_LEDBulbs_DTO saveMCwater(NAAC_AC_714_LEDBulbs_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "LEDBulbsFacade/saveMCwater");
        }

        public NAAC_AC_714_LEDBulbs_DTO EditDataMCwater(NAAC_AC_714_LEDBulbs_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "LEDBulbsFacade/EditDataMCwater");
        }

        public NAAC_AC_714_LEDBulbs_DTO deactivateMCwater(NAAC_AC_714_LEDBulbs_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "LEDBulbsFacade/deactivateMCwater");
        }

        public NAAC_AC_714_LEDBulbs_DTO getDataMCgreen(NAAC_AC_714_LEDBulbs_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "LEDBulbsFacade/getDataMCgreen");
        }
        public NAAC_AC_714_LEDBulbs_DTO saveMCgreen(NAAC_AC_714_LEDBulbs_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "LEDBulbsFacade/saveMCgreen");
        }

        public NAAC_AC_714_LEDBulbs_DTO EditDataMCgreen(NAAC_AC_714_LEDBulbs_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "LEDBulbsFacade/EditDataMCgreen");
        }

        public NAAC_AC_714_LEDBulbs_DTO deactivateMCgreen(NAAC_AC_714_LEDBulbs_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "LEDBulbsFacade/deactivateMCgreen");
        }

        public NAAC_AC_714_LEDBulbs_DTO getDataMCdisable(NAAC_AC_714_LEDBulbs_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "LEDBulbsFacade/getDataMCdisable");
        }
        public NAAC_AC_714_LEDBulbs_DTO saveMCdisable(NAAC_AC_714_LEDBulbs_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "LEDBulbsFacade/saveMCdisable");
        }

        public NAAC_AC_714_LEDBulbs_DTO EditDataMCdisable(NAAC_AC_714_LEDBulbs_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "LEDBulbsFacade/EditDataMCdisable");
        }

        public NAAC_AC_714_LEDBulbs_DTO deactivateMCdisable(NAAC_AC_714_LEDBulbs_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "LEDBulbsFacade/deactivateMCdisable");
        }
        //MC
    }
}
