using CommonLibrary;
using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Medical
{
    public class MC_121_IntDept_CourseDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MC_121_IntDept_Course_DTO, MC_121_IntDept_Course_DTO> COMMM = new CommonDelegate<MC_121_IntDept_Course_DTO, MC_121_IntDept_Course_DTO>();

        public MC_121_IntDept_Course_DTO loaddata(MC_121_IntDept_Course_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "MC_121_IntDept_CourseFacade/loaddata/");
        }

        public MC_121_IntDept_Course_DTO savedata(MC_121_IntDept_Course_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "MC_121_IntDept_CourseFacade/savedata");
        }

        public MC_121_IntDept_Course_DTO editdata(MC_121_IntDept_Course_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "MC_121_IntDept_CourseFacade/editdata");
        }

        public MC_121_IntDept_Course_DTO deactivY(MC_121_IntDept_Course_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "MC_121_IntDept_CourseFacade/deactivY");
        }
        public MC_121_IntDept_Course_DTO get_Course(MC_121_IntDept_Course_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "MC_121_IntDept_CourseFacade/get_Course");
        }  
        public MC_121_IntDept_Course_DTO viewuploadflies(MC_121_IntDept_Course_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "MC_121_IntDept_CourseFacade/viewuploadflies");
        }
        public MC_121_IntDept_Course_DTO deleteuploadfile(MC_121_IntDept_Course_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "MC_121_IntDept_CourseFacade/deleteuploadfile");
        }

        public MC_121_IntDept_Course_DTO savemedicaldatawisecomments(MC_121_IntDept_Course_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "MC_121_IntDept_CourseFacade/savemedicaldatawisecomments");
        }
        public MC_121_IntDept_Course_DTO savefilewisecomments(MC_121_IntDept_Course_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "MC_121_IntDept_CourseFacade/savefilewisecomments");
        }
        public MC_121_IntDept_Course_DTO getcomment(MC_121_IntDept_Course_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "MC_121_IntDept_CourseFacade/getcomment");
        }
        public MC_121_IntDept_Course_DTO getfilecomment(MC_121_IntDept_Course_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "MC_121_IntDept_CourseFacade/getfilecomment");
        }

    }
}
