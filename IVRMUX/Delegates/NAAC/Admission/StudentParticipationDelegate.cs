using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class StudentParticipationDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<NAAC_AC_SParticipation_123_Students_DTO, NAAC_AC_SParticipation_123_Students_DTO> COMMM = new CommonDelegate<NAAC_AC_SParticipation_123_Students_DTO, NAAC_AC_SParticipation_123_Students_DTO>();

        public NAAC_AC_SParticipation_123_Students_DTO loaddata(NAAC_AC_SParticipation_123_Students_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StudentParticipationFacade/loaddata/");
        }

        public NAAC_AC_SParticipation_123_Students_DTO savedata(NAAC_AC_SParticipation_123_Students_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StudentParticipationFacade/savedata");
        }

        public NAAC_AC_SParticipation_123_Students_DTO editdata(NAAC_AC_SParticipation_123_Students_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StudentParticipationFacade/editdata");
        }

        public NAAC_AC_SParticipation_123_Students_DTO deactivY(NAAC_AC_SParticipation_123_Students_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StudentParticipationFacade/deactivY");
        }
        public NAAC_AC_SParticipation_123_Students_DTO get_branch(NAAC_AC_SParticipation_123_Students_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StudentParticipationFacade/get_branch");
        }

        public NAAC_AC_SParticipation_123_Students_DTO get_student(NAAC_AC_SParticipation_123_Students_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StudentParticipationFacade/get_student");
        }

        public NAAC_AC_SParticipation_123_Students_DTO get_MappedStudentList(NAAC_AC_SParticipation_123_Students_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StudentParticipationFacade/get_MappedStudentList");
        }

        public NAAC_AC_SParticipation_123_Students_DTO viewuploadflies(NAAC_AC_SParticipation_123_Students_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StudentParticipationFacade/viewuploadflies");
        }
        public NAAC_AC_SParticipation_123_Students_DTO deleteuploadfile(NAAC_AC_SParticipation_123_Students_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StudentParticipationFacade/deleteuploadfile");
        }
        public NAAC_AC_SParticipation_123_Students_DTO get_coursebrnch(NAAC_AC_SParticipation_123_Students_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StudentParticipationFacade/get_coursebrnch");
        }
        public NAAC_AC_SParticipation_123_Students_DTO savemedicaldatawisecomments(NAAC_AC_SParticipation_123_Students_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StudentParticipationFacade/savemedicaldatawisecomments");
        }
        public NAAC_AC_SParticipation_123_Students_DTO savefilewisecomments(NAAC_AC_SParticipation_123_Students_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StudentParticipationFacade/savefilewisecomments");
        }
        public NAAC_AC_SParticipation_123_Students_DTO getcomment(NAAC_AC_SParticipation_123_Students_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StudentParticipationFacade/getcomment");
        }
        public NAAC_AC_SParticipation_123_Students_DTO getfilecomment(NAAC_AC_SParticipation_123_Students_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "StudentParticipationFacade/getfilecomment");
        }
    }
}
