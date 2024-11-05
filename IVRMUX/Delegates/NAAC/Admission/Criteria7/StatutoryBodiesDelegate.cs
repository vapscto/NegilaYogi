using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class StatutoryBodiesDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<NAAC_AC_7116_StatutoryBodies_DTO, NAAC_AC_7116_StatutoryBodies_DTO> COMMM = new CommonDelegate<NAAC_AC_7116_StatutoryBodies_DTO, NAAC_AC_7116_StatutoryBodies_DTO>();

        public NAAC_AC_7116_StatutoryBodies_DTO loaddata(NAAC_AC_7116_StatutoryBodies_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StatutoryBodiesFacade/loaddata/");
        }

        public NAAC_AC_7116_StatutoryBodies_DTO savedatatab1(NAAC_AC_7116_StatutoryBodies_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StatutoryBodiesFacade/savedatatab1");
        }

        public NAAC_AC_7116_StatutoryBodies_DTO editTab1(NAAC_AC_7116_StatutoryBodies_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StatutoryBodiesFacade/editTab1");
        }
        public NAAC_AC_7116_StatutoryBodies_DTO deactivYTab1(NAAC_AC_7116_StatutoryBodies_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StatutoryBodiesFacade/deactivYTab1");
        }
        public NAAC_AC_7116_StatutoryBodies_DTO getfilecomment(NAAC_AC_7116_StatutoryBodies_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StatutoryBodiesFacade/getfilecomment");
        }
        public NAAC_AC_7116_StatutoryBodies_DTO getcomment(NAAC_AC_7116_StatutoryBodies_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StatutoryBodiesFacade/getcomment");
        }
        public NAAC_AC_7116_StatutoryBodies_DTO savemedicaldatawisecomments(NAAC_AC_7116_StatutoryBodies_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StatutoryBodiesFacade/savemedicaldatawisecomments");
        }
        public NAAC_AC_7116_StatutoryBodies_DTO savefilewisecomments(NAAC_AC_7116_StatutoryBodies_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StatutoryBodiesFacade/savefilewisecomments");
        }

        public NAAC_AC_7116_StatutoryBodies_DTO deleteuploadfile(NAAC_AC_7116_StatutoryBodies_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StatutoryBodiesFacade/deleteuploadfile");
        }

        public NAAC_AC_7116_StatutoryBodies_DTO getData(NAAC_AC_7116_StatutoryBodies_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StatutoryBodiesFacade/getData");
        }
    }
}
