using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class GenderEquityDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<NAAC_AC_711_GenderEquity_DTO, NAAC_AC_711_GenderEquity_DTO> COMMM = new CommonDelegate<NAAC_AC_711_GenderEquity_DTO, NAAC_AC_711_GenderEquity_DTO>();

        public NAAC_AC_711_GenderEquity_DTO loaddata(NAAC_AC_711_GenderEquity_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "GenderEquityFacade/loaddata/");
        }

        public NAAC_AC_711_GenderEquity_DTO savedatatab1(NAAC_AC_711_GenderEquity_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "GenderEquityFacade/savedatatab1");
        }

        public NAAC_AC_711_GenderEquity_DTO editTab1(NAAC_AC_711_GenderEquity_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "GenderEquityFacade/editTab1");
        }

        public NAAC_AC_711_GenderEquity_DTO deactivYTab1(NAAC_AC_711_GenderEquity_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "GenderEquityFacade/deactivYTab1");
        }

        public NAAC_AC_711_GenderEquity_DTO deleteuploadfile(NAAC_AC_711_GenderEquity_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "GenderEquityFacade/deleteuploadfile");
        }
        public NAAC_AC_711_GenderEquity_DTO getcomment(NAAC_AC_711_GenderEquity_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "GenderEquityFacade/getcomment");
        }
        public NAAC_AC_711_GenderEquity_DTO getfilecomment(NAAC_AC_711_GenderEquity_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "GenderEquityFacade/getfilecomment");
        }
        public NAAC_AC_711_GenderEquity_DTO savecomments(NAAC_AC_711_GenderEquity_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "GenderEquityFacade/savecomments");
        }
        public NAAC_AC_711_GenderEquity_DTO savefilewisecomments(NAAC_AC_711_GenderEquity_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "GenderEquityFacade/savefilewisecomments");
        }
        public NAAC_AC_711_GenderEquity_DTO viewuploadflies(NAAC_AC_711_GenderEquity_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "GenderEquityFacade/viewuploadflies");
        }

        public NAAC_AC_711_GenderEquity_DTO getData(NAAC_AC_711_GenderEquity_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "GenderEquityFacade/getData");
        }
    }
}
