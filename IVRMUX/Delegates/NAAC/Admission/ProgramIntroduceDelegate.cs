using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class ProgramIntroduceDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<NAAC_AC_Programs_112_DTO, NAAC_AC_Programs_112_DTO> COMMM = new CommonDelegate<NAAC_AC_Programs_112_DTO, NAAC_AC_Programs_112_DTO>();

        public NAAC_AC_Programs_112_DTO loaddata(NAAC_AC_Programs_112_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProgramIntroduceFacade/loaddata/");
        }
        public NAAC_AC_Programs_112_DTO savedata(NAAC_AC_Programs_112_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProgramIntroduceFacade/savedata");
        }
        public NAAC_AC_Programs_112_DTO editdata(NAAC_AC_Programs_112_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProgramIntroduceFacade/editdata");
        }
        public NAAC_AC_Programs_112_DTO deactivY(NAAC_AC_Programs_112_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProgramIntroduceFacade/deactivY");
        }
        public NAAC_AC_Programs_112_DTO get_Discontinuedflagdata(NAAC_AC_Programs_112_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProgramIntroduceFacade/get_Discontinuedflagdata");
        }
        public NAAC_AC_Programs_112_DTO saveContinued(NAAC_AC_Programs_112_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProgramIntroduceFacade/saveContinued");
        }
        public NAAC_AC_Programs_112_DTO savemappingdata(NAAC_AC_Programs_112_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProgramIntroduceFacade/savemappingdata");
        }
        public NAAC_AC_Programs_112_DTO deactivYTab2(NAAC_AC_Programs_112_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProgramIntroduceFacade/deactivYTab2");
        }
        public NAAC_AC_Programs_112_DTO edittab2(NAAC_AC_Programs_112_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProgramIntroduceFacade/edittab2");
        }
        public NAAC_AC_Programs_112_DTO viewuploadflies(NAAC_AC_Programs_112_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProgramIntroduceFacade/viewuploadflies");
        }
        public NAAC_AC_Programs_112_DTO deleteuploadfile(NAAC_AC_Programs_112_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProgramIntroduceFacade/deleteuploadfile");
        }
        public NAAC_AC_Programs_112_DTO get_branch(NAAC_AC_Programs_112_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProgramIntroduceFacade/get_branch");
        }
        public NAAC_AC_Programs_112_DTO get_program(NAAC_AC_Programs_112_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProgramIntroduceFacade/get_program");
        }
        public NAAC_AC_Programs_112_DTO get_Course(NAAC_AC_Programs_112_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProgramIntroduceFacade/get_Course");
        }
        public NAAC_AC_Programs_112_DTO savemedicaldatawisecomments(NAAC_AC_Programs_112_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProgramIntroduceFacade/savemedicaldatawisecomments");
        }
        public NAAC_AC_Programs_112_DTO savefilewisecomments(NAAC_AC_Programs_112_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProgramIntroduceFacade/savefilewisecomments");
        }
        public NAAC_AC_Programs_112_DTO getcomment(NAAC_AC_Programs_112_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProgramIntroduceFacade/getcomment");
        }
        public NAAC_AC_Programs_112_DTO getfilecomment(NAAC_AC_Programs_112_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProgramIntroduceFacade/getfilecomment");
        }
        //added by sanjeev
        public NAAC_AC_Programs_112_DTO saveadvance(NAAC_AC_Programs_112_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "ProgramIntroduceFacade/saveadvance");
        }
    }
}
