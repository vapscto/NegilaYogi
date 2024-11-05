using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class StaffParticipationDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<NAAC_AC_TParticipation_113_DTO, NAAC_AC_TParticipation_113_DTO> COMMM = new CommonDelegate<NAAC_AC_TParticipation_113_DTO, NAAC_AC_TParticipation_113_DTO>();

        public NAAC_AC_TParticipation_113_DTO loaddata(NAAC_AC_TParticipation_113_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StaffParticipationFacade/loaddata/");
        }

        public NAAC_AC_TParticipation_113_DTO savedata(NAAC_AC_TParticipation_113_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StaffParticipationFacade/savedata");
        }

        public NAAC_AC_TParticipation_113_DTO editdata(NAAC_AC_TParticipation_113_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StaffParticipationFacade/editdata");
        }

        public NAAC_AC_TParticipation_113_DTO deactivY(NAAC_AC_TParticipation_113_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StaffParticipationFacade/deactivY");
        }

        public NAAC_AC_TParticipation_113_DTO get_designation(NAAC_AC_TParticipation_113_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StaffParticipationFacade/get_designation");
        }

        public NAAC_AC_TParticipation_113_DTO get_emp(NAAC_AC_TParticipation_113_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StaffParticipationFacade/get_emp");
        }
        public NAAC_AC_TParticipation_113_DTO viewuploadflies(NAAC_AC_TParticipation_113_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StaffParticipationFacade/viewuploadflies");
        }
        public NAAC_AC_TParticipation_113_DTO deleteuploadfile(NAAC_AC_TParticipation_113_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StaffParticipationFacade/deleteuploadfile");
        }

        public NAAC_AC_TParticipation_113_DTO savemedicaldatawisecomments(NAAC_AC_TParticipation_113_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StaffParticipationFacade/savemedicaldatawisecomments");
        }
        public NAAC_AC_TParticipation_113_DTO savefilewisecomments(NAAC_AC_TParticipation_113_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StaffParticipationFacade/savefilewisecomments");
        }
        public NAAC_AC_TParticipation_113_DTO getcomment(NAAC_AC_TParticipation_113_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StaffParticipationFacade/getcomment");
        }
        public NAAC_AC_TParticipation_113_DTO getfilecomment(NAAC_AC_TParticipation_113_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StaffParticipationFacade/getfilecomment");
        }
        //saveadvance
        public NAAC_AC_TParticipation_113_DTO saveadvance(NAAC_AC_TParticipation_113_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StaffParticipationFacade/saveadvance");
        }
    }
}
