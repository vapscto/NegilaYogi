using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class ProfessionalEthicsDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<NAAC_AC_7115_ProfessionalEthics_DTO, NAAC_AC_7115_ProfessionalEthics_DTO> COMMM = new CommonDelegate<NAAC_AC_7115_ProfessionalEthics_DTO, NAAC_AC_7115_ProfessionalEthics_DTO>();

        public NAAC_AC_7115_ProfessionalEthics_DTO loaddata(NAAC_AC_7115_ProfessionalEthics_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProfessionalEthicsFacade/loaddata/");
        }

        public NAAC_AC_7115_ProfessionalEthics_DTO savedatatab1(NAAC_AC_7115_ProfessionalEthics_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProfessionalEthicsFacade/savedatatab1");
        }

        public NAAC_AC_7115_ProfessionalEthics_DTO savemedicaldatawisecomments(NAAC_AC_7115_ProfessionalEthics_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProfessionalEthicsFacade/savemedicaldatawisecomments");
        }

        public NAAC_AC_7115_ProfessionalEthics_DTO savefilewisecomments(NAAC_AC_7115_ProfessionalEthics_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProfessionalEthicsFacade/savefilewisecomments");
        }

        public NAAC_AC_7115_ProfessionalEthics_DTO getcomment(NAAC_AC_7115_ProfessionalEthics_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProfessionalEthicsFacade/getcomment");
        }

        public NAAC_AC_7115_ProfessionalEthics_DTO getfilecomment(NAAC_AC_7115_ProfessionalEthics_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProfessionalEthicsFacade/getfilecomment");
        }

        public NAAC_AC_7115_ProfessionalEthics_DTO editTab1(NAAC_AC_7115_ProfessionalEthics_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProfessionalEthicsFacade/editTab1");
        }

        public NAAC_AC_7115_ProfessionalEthics_DTO deactivYTab1(NAAC_AC_7115_ProfessionalEthics_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProfessionalEthicsFacade/deactivYTab1");
        }

        public NAAC_AC_7115_ProfessionalEthics_DTO deleteuploadfile(NAAC_AC_7115_ProfessionalEthics_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProfessionalEthicsFacade/deleteuploadfile");
        }

        public NAAC_AC_7115_ProfessionalEthics_DTO getData(NAAC_AC_7115_ProfessionalEthics_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProfessionalEthicsFacade/getData");
        }
    }
}
