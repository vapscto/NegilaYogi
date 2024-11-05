using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class Naac_VACDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<NAAC_AC_VAC_DTO, NAAC_AC_VAC_DTO> COMMM = new CommonDelegate<NAAC_AC_VAC_DTO, NAAC_AC_VAC_DTO>();

        public NAAC_AC_VAC_DTO loaddata(NAAC_AC_VAC_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_VACFacade/loaddata/");
        }
        public NAAC_AC_VAC_DTO savedatatab1(NAAC_AC_VAC_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_VACFacade/savedatatab1");
        }
        public NAAC_AC_VAC_DTO getcommentmaster(NAAC_AC_VAC_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_VACFacade/getcommentmaster");
        }
        public NAAC_AC_VAC_DTO savemedicaldatawisecommentsmaster(NAAC_AC_VAC_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_VACFacade/savemedicaldatawisecommentsmaster");
        }
        public NAAC_AC_VAC_DTO savefilewisecommentsmaster(NAAC_AC_VAC_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_VACFacade/savefilewisecommentsmaster");
        }
        public NAAC_AC_VAC_DTO getfilecommentmaster(NAAC_AC_VAC_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_VACFacade/getfilecommentmaster");
        }
        public NAAC_AC_VAC_DTO editTab1(NAAC_AC_VAC_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_VACFacade/editTab1");
        }
        public NAAC_AC_VAC_DTO deactivYTab1(NAAC_AC_VAC_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_VACFacade/deactivYTab1");
        }
        public NAAC_AC_VAC_DTO get_student(NAAC_AC_VAC_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_VACFacade/get_student");
        }
        public NAAC_AC_VAC_DTO savedatatab2(NAAC_AC_VAC_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_VACFacade/savedatatab2");
        }
        public NAAC_AC_VAC_DTO getcomment(NAAC_AC_VAC_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_VACFacade/getcomment");
        }
        public NAAC_AC_VAC_DTO getfilecomment(NAAC_AC_VAC_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_VACFacade/getfilecomment");
        }
        public NAAC_AC_VAC_DTO savefilewisecomments(NAAC_AC_VAC_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_VACFacade/savefilewisecomments");
        }
        public NAAC_AC_VAC_DTO savemedicaldatawisecomments(NAAC_AC_VAC_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_VACFacade/savemedicaldatawisecomments");
        }
        public NAAC_AC_VAC_DTO edittab2(NAAC_AC_VAC_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_VACFacade/edittab2");
        }
        public NAAC_AC_VAC_DTO deactivYTabstudent(NAAC_AC_VAC_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_VACFacade/deactivYTabstudent");
        }
        public NAAC_AC_VAC_DTO viewstudent(NAAC_AC_VAC_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_VACFacade/viewstudent");
        }
        public NAAC_AC_VAC_DTO deactivYTab2(NAAC_AC_VAC_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_VACFacade/deactivYTab2");
        }
        public NAAC_AC_VAC_DTO get_Mappedstudentlist(NAAC_AC_VAC_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_VACFacade/get_Mappedstudentlist");
        }
        public NAAC_AC_VAC_DTO get_Continuedflagdata(NAAC_AC_VAC_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_VACFacade/get_Continuedflagdata");
        }
        public NAAC_AC_VAC_DTO saveContinued(NAAC_AC_VAC_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_VACFacade/saveContinued");
        }
        public NAAC_AC_VAC_DTO get_Completedflagdata(NAAC_AC_VAC_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_VACFacade/get_Completedflagdata");
        }
        public NAAC_AC_VAC_DTO saveCompletedflag(NAAC_AC_VAC_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_VACFacade/saveCompletedflag");
        }
        public NAAC_AC_VAC_DTO viewuploadfliesmain(NAAC_AC_VAC_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_VACFacade/viewuploadfliesmain");
        }
        public NAAC_AC_VAC_DTO deletemainfile(NAAC_AC_VAC_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_VACFacade/deletemainfile");
        }
        public NAAC_AC_VAC_DTO viewuploadfliesstudent(NAAC_AC_VAC_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_VACFacade/viewuploadfliesstudent");
        }
        public NAAC_AC_VAC_DTO deletestudentfiles(NAAC_AC_VAC_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_VACFacade/deletestudentfiles");
        }
        //added by saNJEEV
        public NAAC_AC_VAC_DTO saveadvance(NAAC_AC_VAC_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_VACFacade/saveadvance");
        }
    }
}
