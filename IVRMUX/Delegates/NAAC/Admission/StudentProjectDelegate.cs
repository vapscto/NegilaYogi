using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class StudentProjectDelegate
    {
        CommonDelegate<StudentProject_DTO, StudentProject_DTO> comm = new CommonDelegate<StudentProject_DTO, StudentProject_DTO>();
        public StudentProject_DTO loaddata(StudentProject_DTO data)
        {
            return comm.naacdetailsbypost(data, "StudentProjectFacade/loaddata");
        }
        public StudentProject_DTO savedata(StudentProject_DTO data)
        {
            return comm.naacdetailsbypost(data, "StudentProjectFacade/savedata");
        }
        public StudentProject_DTO editdata(StudentProject_DTO data)
        {
            return comm.naacdetailsbypost(data, "StudentProjectFacade/editdata");
        }
        public StudentProject_DTO deactiveStudent(StudentProject_DTO data)
        {
            return comm.naacdetailsbypost(data, "StudentProjectFacade/deactiveStudent");
        }
        public StudentProject_DTO get_branch(StudentProject_DTO data)
        {
            return comm.naacdetailsbypost(data, "StudentProjectFacade/get_branch");
        }
        public StudentProject_DTO get_student(StudentProject_DTO data)
        {
            return comm.naacdetailsbypost(data, "StudentProjectFacade/get_student");
        }
        public StudentProject_DTO viewuploadflies(StudentProject_DTO data)
        {
            return comm.naacdetailsbypost(data, "StudentProjectFacade/viewuploadflies");
        }
        public StudentProject_DTO deleteuploadfile(StudentProject_DTO data)
        {
            return comm.naacdetailsbypost(data, "StudentProjectFacade/deleteuploadfile");
        }


        public StudentProject_DTO MC_Savedata_134(StudentProject_DTO data)
        {
            return comm.naacdetailsbypost(data, "StudentProjectFacade/MC_Savedata_134");
        }
        public StudentProject_DTO MC_editdata_134(StudentProject_DTO data)
        {
            return comm.naacdetailsbypost(data, "StudentProjectFacade/MC_editdata_134");
        }
        public StudentProject_DTO MC_viewuploadflies_134(StudentProject_DTO data)
        {
            return comm.naacdetailsbypost(data, "StudentProjectFacade/MC_viewuploadflies_134");
        }
        public StudentProject_DTO MC_deleteuploadfile_134(StudentProject_DTO data)
        {
            return comm.naacdetailsbypost(data, "StudentProjectFacade/MC_deleteuploadfile_134");
        }


        public StudentProject_DTO savemedicaldatawisecomments(StudentProject_DTO data)
        {
            return comm.naacdetailsbypost(data, "StudentProjectFacade/savemedicaldatawisecomments");
        }
        public StudentProject_DTO savefilewisecomments(StudentProject_DTO data)
        {
            return comm.naacdetailsbypost(data, "StudentProjectFacade/savefilewisecomments");
        }
        public StudentProject_DTO getcomment(StudentProject_DTO data)
        {
            return comm.naacdetailsbypost(data, "StudentProjectFacade/getcomment");
        }
        public StudentProject_DTO getfilecomment(StudentProject_DTO data)
        {
            return comm.naacdetailsbypost(data, "StudentProjectFacade/getfilecomment");
        }



        public StudentProject_DTO savedatawisecommentsAffi(StudentProject_DTO data)
        {
            return comm.naacdetailsbypost(data, "StudentProjectFacade/savedatawisecommentsAffi");
        }
        public StudentProject_DTO savefilewisecommentsAffi(StudentProject_DTO data)
        {
            return comm.naacdetailsbypost(data, "StudentProjectFacade/savefilewisecommentsAffi");
        }
        public StudentProject_DTO getcommentAffi(StudentProject_DTO data)
        {
            return comm.naacdetailsbypost(data, "StudentProjectFacade/getcommentAffi");
        }
        public StudentProject_DTO getfilecommentAffi(StudentProject_DTO data)
        {
            return comm.naacdetailsbypost(data, "StudentProjectFacade/getfilecommentAffi");
        }


        public StudentProject_DTO deactiveY(StudentProject_DTO data)
        {
            return comm.naacdetailsbypost(data, "StudentProjectFacade/deactiveY");
        }
        //added by sanjeev
        public StudentProject_DTO saveadvance(StudentProject_DTO data)
        {
            return comm.naacdetailsbypost(data, "StudentProjectFacade/saveadvance");
        }
        
    }
}
